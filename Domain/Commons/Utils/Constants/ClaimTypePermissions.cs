using Domain.Commons.Enums;

namespace Domain.Commons.Utils.Constants;

public static class ClaimTypePermissions
{
    public static readonly Dictionary<ClaimType, HashSet<ClaimValue>> AllowedValues = new()
    {
        { ClaimType.Appointment, [ClaimValue.None, ClaimValue.Read, ClaimValue.Create, ClaimValue.Update, ClaimValue.Delete]},
        { ClaimType.AppointmentCategory, [ClaimValue.None, ClaimValue.Read, ClaimValue.Create, ClaimValue.Update, ClaimValue.Delete]},
        { ClaimType.AppointmentProtocol, [ClaimValue.None, ClaimValue.Read, ClaimValue.Update]},
        { ClaimType.Result, [ClaimValue.None, ClaimValue.Read, ClaimValue.Create, ClaimValue.Delete]},
        
        { ClaimType.Clinician, [ClaimValue.None, ClaimValue.Read, ClaimValue.Create, ClaimValue.Update, ClaimValue.Archive, ClaimValue.Delete]},
        { ClaimType.ClinicianCategory, [ClaimValue.None, ClaimValue.Read, ClaimValue.Create, ClaimValue.Update, ClaimValue.Delete]},
        
        { ClaimType.Device, [ClaimValue.None, ClaimValue.Read, ClaimValue.Create, ClaimValue.Update, ClaimValue.Archive, ClaimValue.Delete]},
        { ClaimType.DeviceCategory, [ClaimValue.None, ClaimValue.Read, ClaimValue.Create, ClaimValue.Update, ClaimValue.Delete]},
        
        { ClaimType.Clinic, [ClaimValue.None, ClaimValue.Update]},
        { ClaimType.Role, [ClaimValue.None, ClaimValue.Read, ClaimValue.Create, ClaimValue.Update, ClaimValue.Delete]},
        { ClaimType.User, [ClaimValue.None, ClaimValue.Read, ClaimValue.Create, ClaimValue.Update, ClaimValue.Archive, ClaimValue.Delete]},
        
        { ClaimType.Patient, [ClaimValue.None, ClaimValue.Read, ClaimValue.Create, ClaimValue.Update, ClaimValue.Archive, ClaimValue.Delete]},
        
        { ClaimType.Room, [ClaimValue.None, ClaimValue.Read, ClaimValue.Create, ClaimValue.Update, ClaimValue.Archive, ClaimValue.Delete]},
        { ClaimType.RoomCategory, [ClaimValue.None, ClaimValue.Read, ClaimValue.Create, ClaimValue.Update, ClaimValue.Delete]},
    };

    public static bool IsAllowed(ClaimType claimType, ClaimValue claimValue)
        => AllowedValues.TryGetValue(claimType, out var allowedValues) && allowedValues.Contains(claimValue);
}