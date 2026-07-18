using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application;
using Application.Common.Configuration;
using Infrastructure;
using Infrastructure.Common.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PMS_ClinicAPI;
using PMS_ClinicAPI.Common.Authorization;
using PMS_ClinicAPI.Common.Exceptions;
using PMS_ClinicAPI.Common.Logging;
using PMS_ClinicAPI.Common.Middleware;
using PMS_ClinicAPI.Common.Utils.Conversions.DateTimes;
using PMS_ClinicAPI.Common.Utils.Conversions.Enums;
using PMS_ClinicAPI.Common.Utils.Returns;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;
using Utils.Authentication;

/* - - - Configuring services  - - - */

// Initializing builder object
var builder = WebApplication.CreateBuilder(args);

// Registering configuration classes
builder.Services.AddOptions<TokenLifetimeSettings>()
    .BindConfiguration(TokenLifetimeSettings.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddOptions<CookieSettings>()
    .BindConfiguration(CookieSettings.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddOptions<JwtSettings>()
    .BindConfiguration(JwtSettings.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();

// Adding logging by creating configuration
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Host.UseSerilog();

// Adding cors policy
var corsSettings = builder.Configuration
    .GetSection(CorsSettings.SectionName)
    .Get<CorsSettings>() ?? throw new InvalidConfigurationException(CorsSettings.SectionName); 
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policyBuilder =>
    {
        policyBuilder.WithOrigins(corsSettings.AllowedOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services
    // Adding controllers
    .AddControllers()
    
    // Configuring api behaviour options
    .ConfigureApiBehaviorOptions(options =>
    {
        // Configuring custom automatic response on invalid models
        options.InvalidModelStateResponseFactory = context =>
        {
            // Extracting model binding errors
            var modelBindingErrors = context.ModelState
                .Where(modelState => modelState.Value?.Errors.Count > 0)
                .SelectMany(modelStateEntry => modelStateEntry.Value!.Errors.Select(error => $"{modelStateEntry.Key}: {error.ErrorMessage}"))
                .ToList();

            // Creating exception and http result   
            var inputModelValidationException = new InputModelValidationException(modelBindingErrors);
            var httpResult = new HttpResult<EmptyPayload>(inputModelValidationException);
            Log.Warning(LogMessages.InputModelValidationFailed, string.Join(", ", modelBindingErrors));
            
            // Returning result
            return new JsonResult(httpResult)
            {
                StatusCode = httpResult.HttpStatusCode
            };
        };
    })
    
    // Configuring json options
    .AddJsonOptions(options =>
    {
        // Configuring enum converter
        options.JsonSerializerOptions.Converters.Add(new StrictEnumConverterFactory());

        // Configuring utc date time converter
        options.JsonSerializerOptions.Converters.Add(new UtcDateTimeConverter());
        
        // Configuring loop handling 
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

        // Configuring camel casing
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

// Adding layer dependencies
builder.Services.AddApplicationLayerDependencies();
builder.Services.AddInfrastructureLayerDependencies(builder.Configuration
    .GetSection(DatabaseSettings.SectionName)
    .Get<DatabaseSettings>() ?? throw new InvalidConfigurationException(DatabaseSettings.SectionName));
builder.Services.AddApiLayerDependencies();

// Adding jwt authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Accessing configuration
        var jwtSettings = builder.Configuration
            .GetSection(JwtSettings.SectionName)
            .Get<JwtSettings>() ?? throw new InvalidConfigurationException(JwtSettings.SectionName);

        // Configuring token validation parameters
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
            ClockSkew = TimeSpan.Zero
        };

        // Configuring necessary claims
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                // Retrieving claims
                var claims = context.Principal?.Claims.ToList();

                // Extracting mandatory claims
                var clinicIdClaim = claims?.FirstOrDefault(claim => claim.Type == ClaimNames.ClinicId);
                var userIdClaim = claims?.FirstOrDefault(claim => claim.Type == ClaimNames.UserId);

                // Checking mandatory claims
                if (clinicIdClaim == null || !Guid.TryParse(clinicIdClaim.Value, out _) ||
                    userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out _))
                {
                    Log.Error(LogMessages.MissingMandatoryClaim, ClaimNames.ClinicId, ClaimNames.UserId);
                    context.Fail($"Required claims {ClaimNames.ClinicId} and {ClaimNames.UserId} are missing");
                }

                // Returning task
                return Task.CompletedTask;
            }
        }; 
    });

// Adding authorization by adding custom policies
builder.Services.AddAuthorization(PolicyDefinitions.AddPolicies);

// Adding Swagger
builder.Services.AddSwaggerGen(options =>
{
    // Enabling annotations
    options.EnableAnnotations();

    // Adding security definitions
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Adding security requirements
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "Bearer",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            Array.Empty<string>()
        }
    });

    // Ordering endpoints
    options.OrderActionsBy(action => action.HttpMethod);
});

/* - - - Registering middleware  - - - */

// Building web application
var app = builder.Build();

// Checking database connectivity
using (var scope = app.Services.CreateScope())
{
    // Getting database context
    var databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    
    // Checking if connection can be established
    try
    {
        // Migrating database
        await databaseContext.Database.MigrateAsync();
        Log.Logger.Information(LogMessages.DatabaseMigrationsSucceeded);
    }
    catch (Exception)
    {
        Log.Fatal(LogMessages.DatabaseConnectionFailed);
        await Log.CloseAndFlushAsync();
        return;
    }
}

// Checking environment
if (app.Environment.IsDevelopment())
{
    // Using developer exception page
    app.UseDeveloperExceptionPage();

    // Using Swagger
    app.UseSwagger(options => { options.RouteTemplate = "api/docs/{documentName}/swagger.json"; });

    // Using SwaggerUI
    app.UseSwaggerUI(options =>
    {
        // Activating automatic inclusion of request credentials
        options.ConfigObject.AdditionalItems["requestCredentials"] = "include"; 
        
        // Setting endpoint and route prefix
        options.SwaggerEndpoint("v1/swagger.json", "ClinicAPI v1");
        options.RoutePrefix = "api/docs";

        // Applying additional settings
        options.DocumentTitle = "ClinicAPI Documentation";
        options.DisplayRequestDuration();
        options.DefaultModelsExpandDepth(0);
        options.DocExpansion(DocExpansion.None);
    });
}
else
{
    // Using http strict transport security
    app.UseHsts();
}

// Using https redirection
app.UseHttpsRedirection();

// Using routing
app.UseRouting();

// Using request logging context middleware
app.UseMiddleware<RequestLoggingContextMiddleware>();

// Using global exception handler
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// Using cors
app.UseCors("CorsPolicy");

// Using authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

// Mapping controllers
app.MapControllers();

// Starting application
app.Run();