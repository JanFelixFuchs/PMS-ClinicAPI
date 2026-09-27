using Domain.Common.Enums;
using Domain.Common.Utils.Constants;
using FluentAssertions;

namespace Domain.Tests.Tests.Common.Utils.Constants;

public class ClaimTypePermissionsTests
{
    /* - - - Property: AllowedValues - - - */
    [Fact]
    public void AllowedValues_HasEntryForEveryDefinedClaimType()
    {
        foreach (var claimType in Enum.GetValues<ClaimType>())
        {
            // Assert
            ClaimTypePermissions.AllowedValues.Should().ContainKey(
                claimType, 
                $"because {claimType} should have configured allowed values");
        }
    }

    [Fact]
    public void AllowedValues_EveryClaimTypeAllowsNone()
    {
        foreach (var (claimType, allowedValues) in ClaimTypePermissions.AllowedValues)
        {
            // Assert
            allowedValues.Should().Contain(
                ClaimValue.None,
                $"because {claimType} should always allow {ClaimValue.None} as a permission");
        }
    }

    [Fact]
    public void AllowedValues_NoClaimTypeAllowsUndefinedClaimValues()
    {
        // Arrange
        var definedClaimValues = Enum.GetValues<ClaimValue>().ToHashSet();
        
        foreach (var (claimType, allowedValues) in ClaimTypePermissions.AllowedValues)
        {
            // Assert
            allowedValues.Should().BeSubsetOf(
                definedClaimValues,
                $"because {claimType} should not reference undefined {nameof(ClaimValue)} entries");
        }
    }
    
    
    /* - - - Method: IsAllowed - - - */
    [Fact]
    public void IsAllowed_WithUndefinedClaimType_ReturnsFalse()
    {
        // Act
        var result = ClaimTypePermissions.IsAllowed((ClaimType)999, ClaimValue.None);
        
        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsAllowed_WithUndefinedClaimValue_ReturnsFalse()
    {
        // Act
        var result = ClaimTypePermissions.IsAllowed(ClaimType.Clinician, (ClaimValue)999);
        
        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(ClaimType.Appointment, ClaimValue.Archive)]
    [InlineData(ClaimType.AppointmentCategory, ClaimValue.Archive)]
    [InlineData(ClaimType.AppointmentProtocol, ClaimValue.Create)]
    [InlineData(ClaimType.AppointmentProtocol, ClaimValue.Archive)]
    [InlineData(ClaimType.AppointmentProtocol, ClaimValue.Delete)]
    [InlineData(ClaimType.Result, ClaimValue.Update)]
    [InlineData(ClaimType.Result, ClaimValue.Archive)]
    [InlineData(ClaimType.ClinicianCategory, ClaimValue.Archive)]
    [InlineData(ClaimType.DeviceCategory, ClaimValue.Archive)]
    [InlineData(ClaimType.Clinic, ClaimValue.Create)]
    [InlineData(ClaimType.Clinic, ClaimValue.Archive)]
    [InlineData(ClaimType.Clinic, ClaimValue.Delete)]
    [InlineData(ClaimType.Role, ClaimValue.Archive)]
    [InlineData(ClaimType.RoomCategory, ClaimValue.Archive)]
    public void IsAllowed_WithClaimValueNotInAllowedSet_ReturnsFalse(ClaimType claimType, ClaimValue claimValue)
    {
        // Act
        var result = ClaimTypePermissions.IsAllowed(claimType, claimValue);
        
        // Assert
        result.Should().BeFalse();
    }
}