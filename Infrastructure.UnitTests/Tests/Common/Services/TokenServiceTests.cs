using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Application.Common.Providers;
using Application.Common.Services;
using Domain.Entities.IdentityEntities;
using FluentAssertions;
using Infrastructure.Common.Configuration;
using Infrastructure.Common.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NSubstitute;
using TestUtils.Builders.AuthBuilders;
using TestUtils.Constants;
using Utils.Authentication;

namespace Infrastructure.UnitTests.Tests.Common.Services;

public class TokenServiceTests
{
    /* - - - Preparation - - - */
    public static TheoryData<List<Claim>> ValidClaims =>
    [
        new List<Claim>(),
        new List<Claim>{ TestClaimBuilder.Create().Build()}
    ];
    
    private readonly JwtSettings _jwtSettings = new()
    {
        Issuer = "test-issuer",
        Audience = "test-audience",
        Secret = "test-secret-which-is-very-very-very-very-very-very-very-very-very-long",
        AccessTokenLifetimeInMinutes = 15,
        RefreshTokenLifetimeInDays = 1
    };
    private readonly ITokenService _sut;
    private readonly IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();
    
    public TokenServiceTests()
    {
        _dateTimeProvider.UtcNow.Returns(TestConstants.DefaultCurrentDateTime);
        
        _sut = new TokenService(Options.Create(_jwtSettings), _dateTimeProvider);
    }
    
    
    /* - - - Method: CreateAccessToken - - - */
    [Fact]
    public void CreateAccessToken_WithNullClaims_ThrowsArgumentNullException()
    {
        // Act
        var act = () => _sut.CreateAccessToken(Guid.NewGuid(), Guid.NewGuid(), null!);
        
        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
    
    [Theory]
    [MemberData(nameof(ValidClaims))]
    public void CreateAccessToken_WithValidArguments_ReturnsAccessTokenContainingClaims(ICollection<Claim> claims)
    {
        // Arrange
        var clinicId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        // Act
        var result = _sut.CreateAccessToken(clinicId, userId, claims);

        // Assert
        var accessToken = new JwtSecurityTokenHandler().ReadJwtToken(result);

        accessToken.Claims.Should().ContainSingle(claim =>
            claim.Type == ClaimNames.ClinicId &&
            claim.Value == clinicId.ToString());
        
        accessToken.Claims.Should().ContainSingle(claim =>
            claim.Type == ClaimNames.UserId &&
            claim.Value == userId.ToString());
        
        foreach (var expectedClaim in claims)
        {
            accessToken.Claims.Should().ContainSingle(claim =>
                claim.Type == $"{ClaimNames.PermissionPrefix}{expectedClaim.Type}" &&
                claim.Value == expectedClaim.Value.ToString());
        }
    }
    
    [Fact]
    public void CreateAccessToken_WithValidArguments_ReturnsTokenWithCorrectIssuerAndAudience()
    {
        // Act
        var result = _sut.CreateAccessToken(Guid.NewGuid(), Guid.NewGuid(), []);

        // Assert
        var accessToken = new JwtSecurityTokenHandler().ReadJwtToken(result);
        accessToken.Issuer.Should().Be(_jwtSettings.Issuer);
        accessToken.Audiences.Should().ContainSingle().Which.Should().Be(_jwtSettings.Audience);
    }
    
    [Fact]
    public void CreateAccessToken_WithValidArguments_ReturnsTokenThatPassesValidation()
    {
        // Arrange
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _jwtSettings.Issuer,
            
            ValidateAudience = true,
            ValidAudience = _jwtSettings.Audience,
            
            ValidateLifetime = false,
            
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
        };

        // Act
        var result = _sut.CreateAccessToken(Guid.NewGuid(), Guid.NewGuid(), []);

        // Assert
        var act = () => new JwtSecurityTokenHandler().ValidateToken(result, tokenValidationParameters, out _);
        act.Should().NotThrow();
    }
    
    [Fact]
    public void CreateAccessToken_WithValidArguments_ReturnsTokenWithCorrectExpiration()
    {
        // Act
        var result = _sut.CreateAccessToken(Guid.NewGuid(), Guid.NewGuid(), []);

        // Assert
        var accessToken = new JwtSecurityTokenHandler().ReadJwtToken(result);
        accessToken.ValidTo.Should().Be(_dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.AccessTokenLifetimeInMinutes));
    }
    
    
    /* - - - Method: CreateRefreshToken - - - */
    [Fact]
    public void CreateRefreshToken_ReturnsEncodedRefreshToken()
    {
        // Act
        var result = _sut.CreateRefreshToken();
        
        // Assert
        var bytes = Convert.FromBase64String(result);
        bytes.Should().HaveCount(64);
    }
}