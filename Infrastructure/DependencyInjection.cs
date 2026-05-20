using Application.Common.Contexts;
using Application.Common.Services;
using Application.Common.Transactions;
using Application.Repositories.AppointmentRepositories;
using Application.Repositories.ClinicianRepositories;
using Application.Repositories.DeviceRepositories;
using Application.Repositories.IdentityRepositories;
using Application.Repositories.PatientRepositories;
using Application.Repositories.RoomRepositories;
using Infrastructure.Common.Contexts;
using Infrastructure.Common.Services;
using Infrastructure.Common.Transactions;
using Infrastructure.Repositories.AppointmentRepositories;
using Infrastructure.Repositories.ClinicianRepositories;
using Infrastructure.Repositories.DeviceRepositories;
using Infrastructure.Repositories.IdentityRepositories;
using Infrastructure.Repositories.PatientRepositories;
using Infrastructure.Repositories.RoomRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AuthenticationService = Infrastructure.Common.Services.AuthenticationService;
using IAuthenticationService = Application.Common.Services.IAuthenticationService;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureLayerDependencies(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        // Building connection string
        var databaseCredentials = configuration.GetSection("DatabaseSettings");
        var connectionString = $"Server={databaseCredentials.GetValue<string>("Server")};" +
                               $"Database={databaseCredentials.GetValue<string>("Database")};" +
                               $"Uid={databaseCredentials.GetValue<string>("User")};" +
                               $"Pwd={databaseCredentials.GetValue<string>("Password")};" +
                               $"Port={databaseCredentials.GetValue<string>("Port")}";
        
        // Adding database context
        serviceCollection.AddDbContext<DatabaseContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), builder =>
            {
                builder.MigrationsAssembly("ClinicAPI");
            }));
        
        // Adding transients for repositories
        serviceCollection.AddTransient<IAppointmentRepository, AppointmentRepository>();
        serviceCollection.AddTransient<IAppointmentCategoryRepository, AppointmentCategoryRepository>();
        serviceCollection.AddTransient<IAppointmentProtocolRepository, AppointmentProtocolRepository>();
        serviceCollection.AddTransient<IClinicianCategoryRepository, ClinicianCategoryRepository>();
        serviceCollection.AddTransient<IClinicianRepository, ClinicianRepository>();
        serviceCollection.AddTransient<IDeviceCategoryRepository, DeviceCategoryRepository>();
        serviceCollection.AddTransient<IDeviceRepository, DeviceRepository>();
        serviceCollection.AddTransient<IClaimRepository, ClaimRepository>();
        serviceCollection.AddTransient<IClinicRepository, ClinicRepository>();
        serviceCollection.AddTransient<IRoleRepository, RoleRepository>();
        serviceCollection.AddTransient<IUserRepository, UserRepository>();
        serviceCollection.AddTransient<IPatientRepository, PatientRepository>();
        serviceCollection.AddTransient<IRoomRepository, RoomRepository>();
        serviceCollection.AddTransient<IRoomCategoryRepository, RoomCategoryRepository>();
        
        // Adding transients for services
        serviceCollection.AddTransient<IAuthenticationService, AuthenticationService>();
        serviceCollection.AddTransient<ITokenService, TokenService>();
        
        // Adding transient for unit of work
        serviceCollection.AddTransient<IUnitOfWork, UnitOfWork>();
        
        // Adding transient for contexts
        serviceCollection.AddScoped<IRequestContext, RequestContext>();
    }
}