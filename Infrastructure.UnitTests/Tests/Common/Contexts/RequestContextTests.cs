using System.Security.Claims;
using FluentAssertions;
using Infrastructure.Common.Contexts;
using Infrastructure.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Utils.Authentication;

namespace Infrastructure.UnitTests.Tests.Common.Contexts;

public class RequestContextTests
{
    /* - - - Preparation - - - */
    public static TheoryData<string, Func<RequestContext, Guid>> GuidPropertySelectorsWithClaimTypes => new()
    {
        { ClaimNames.ClinicId, sut => sut.ClinicId },
        { ClaimNames.UserId, sut => sut.UserId }
    };
    
    private static RequestContext CreateSut(params (string type, string value)[]? claims)
    {
        var httpAccessor = Substitute.For<IHttpContextAccessor>();

        if (claims is null)
        {
            httpAccessor.HttpContext.Returns((HttpContext?)null);
        }
        else
        {
            var claimsIdentity = new ClaimsIdentity(claims.Select(claim => new Claim(claim.type, claim.value)));
            var defaultHttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(claimsIdentity) };
            
            httpAccessor.HttpContext.Returns(defaultHttpContext);
        }
        
        return new RequestContext(httpAccessor);
    }
    
    
    /* - - - Property: Id - - - */
    [Theory]
    [MemberData(nameof(GuidPropertySelectorsWithClaimTypes))]
    public void Id_WithMissingClaim_ThrowsInvalidRequestContextException(string _, Func<RequestContext, Guid> claimSelector)
    {
        // Arrange
        var sut = CreateSut();
        
        // Act
        var act = () => claimSelector(sut);
        
        // Assert
        act.Should().Throw<InvalidRequestContextException>();
    }
    
    [Theory]
    [MemberData(nameof(GuidPropertySelectorsWithClaimTypes))]
    public void Id_WithNonGuidClaimValue_ThrowsInvalidRequestContextException(
        string claimType,
        Func<RequestContext, Guid> claimSelector)
    {
        // Arrange
        const string nonGuidClaimValue = "non-guid-claim-value";
        var sut = CreateSut((claimType, nonGuidClaimValue));
        
        // Act
        var act = () => claimSelector(sut);
        
        // Assert
        act.Should().Throw<InvalidRequestContextException>();
    }
    
    [Theory]
    [MemberData(nameof(GuidPropertySelectorsWithClaimTypes))]
    public void Id_WithNullHttpContext_ThrowsInvalidRequestContextException(string _, Func<RequestContext, Guid> claimSelector)
    {
        // Arrange
        var sut = CreateSut(null);
        
        // Act
        var act = () => claimSelector(sut);
        
        // Assert
        act.Should().Throw<InvalidRequestContextException>();
    }
    
    [Theory]
    [MemberData(nameof(GuidPropertySelectorsWithClaimTypes))]
    public void Id_WithValidClaim_ReturnsGuid(string claimType, Func<RequestContext, Guid> claimSelector)
    {
        // Arrange
        var id = Guid.NewGuid();
        var sut = CreateSut((claimType, id.ToString()));
        
        // Act
        var result = claimSelector(sut);
        
        // Assert
        result.Should().Be(id);
    }
}