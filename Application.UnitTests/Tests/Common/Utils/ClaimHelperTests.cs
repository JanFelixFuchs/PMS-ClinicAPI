using Application.Common.Utils;
using Domain.Common.Enums;
using Domain.Common.Utils.Constants;
using Domain.Entities.IdentityEntities;
using FluentAssertions;
using TestUtils.Builders.AuthBuilders;

namespace Application.UnitTests.Tests.Common.Utils;

public class ClaimHelperTests
{
    /* - - - Method: CreateHighestPermissionClaims - - - */
    [Fact]
    public void CreateHighestPermissionClaims_WithValidRole_CreatesHighestPermissionClaimsForEveryClaimType()
    {
        // Arrange
        var role = TestRoleBuilder.Create().Build();
        
        // Act
        var result = ClaimHelper.CreateHighestPermissionClaims(role);
        
        // Assert
        result.Should().HaveCount(ClaimTypePermissions.AllowedValues.Count);
        foreach (var allowedValue in ClaimTypePermissions.AllowedValues)
        {
            result.Should().ContainSingle(claim =>
                claim.Role == role &&
                claim.Type == allowedValue.Key &&
                claim.Value == allowedValue.Value.Max());
        }
    }
    
    
    /* - - - Method: CreateClaimsFromDictionary - - - */
    [Fact]
    public void CreateClaimsFromDictionary_WithNullClaims_ThrowsArgumentNullException()
    {
        // Act
        var act = () => ClaimHelper.CreateClaimsFromDictionary(
            TestRoleBuilder.Create().Build(),
            null!);
        
        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
    
    [Fact]
    public void CreateClaimsFromDictionary_WithValidArguments_CreatesCorrectClaimsDictionary()
    {
        // Arrange
        var role = TestRoleBuilder.Create().Build();
        var claims = new Dictionary<ClaimType, ClaimValue>
        {
            { ClaimType.Appointment, ClaimValue.None },
            { ClaimType.AppointmentCategory, ClaimValue.Read },
            { ClaimType.AppointmentProtocol, ClaimValue.Update },
            { ClaimType.Result, ClaimValue.Archive },
            { ClaimType.Clinician, ClaimValue.Delete }
        };
        
        // Act
        var result = ClaimHelper.CreateClaimsFromDictionary(role, claims);
        
        // Assert
        result.Should().HaveCount(claims.Count);
        foreach (var claimEntry in claims)
        {
            result.Should().ContainSingle(claim =>
                claim.Role == role &&
                claim.Type == claimEntry.Key &&
                claim.Value == claimEntry.Value);
        }
    }
    
    
    /* - - - Method: FillMissingClaimsWithLowestPermission - - - */
    [Fact]
    public void FillMissingClaimsWithLowestPermission_WithNullClaims_ThrowsArgumentNullException()
    {
        // Act
        var act = () => ClaimHelper.FillMissingClaimsWithLowestPermission(
            TestRoleBuilder.Create().Build(),
            null!);
        
        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
    
    [Fact]
    public void FillMissingClaimsWithLowestPermission_WithValidArguments_FillsMissingClaimsWithLowestPermission()
    {
        // Arrange
        var role = TestRoleBuilder.Create().Build();
        var existingClaim = TestClaimBuilder.Create().Build();
        
        // Act
        var result = ClaimHelper.FillMissingClaimsWithLowestPermission(role, new List<Claim> { existingClaim });
        
        // Assert
        result.Should().HaveCount(ClaimTypePermissions.AllowedValues.Count);
        result.Select(claim => claim.Type).Should().BeEquivalentTo(ClaimTypePermissions.AllowedValues.Keys);
        result.Single(claim => claim.Type == existingClaim.Type).Value.Should().Be(existingClaim.Value);
        result
            .Where(claim => claim.Type != existingClaim.Type)
            .Should().AllSatisfy(claim =>
            {
                claim.Role.Should().Be(role);
                claim.Value.Should().Be(ClaimTypePermissions.AllowedValues[claim.Type].Min());
            });
    }
}