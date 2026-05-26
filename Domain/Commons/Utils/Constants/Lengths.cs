namespace Domain.Commons.Utils.Constants;

public static class Lengths
{
    public const int Code = 256;
    public const int NormalizedCode = Code;

    private const int Name = 256;
    public const int CategoryName = Name;
    public const int ClinicName = Name;
    public const int RoleName = Name;
    public const int NormalizedRoleName = RoleName;
    public const int Username = Name;
    public const int NormalizedUsername = Username;
    public const int DeviceName = Name;
    public const int RoomName = Name;
    public const int FirstName = Name;
    public const int LastName = Name;

    public const int PasswordHash = 256;

    public const int RefreshTokenHash = 128;

    public const int ClaimType = 128;
    public const int ClaimValue = 128;

    public const int Abbreviation = 64;

    public const int Color = 8;

    public const int MedicalField = 128;
    public const int Owner = 256;

    public const int Email = 256;
    public const int PhoneNumber = 32;
    
    public const int Street = 256;
    public const int HouseNumber = 8;
    public const int City = 128;
    public const int ZipCode = 8;
    public const int Country = 4;

    public const int AppointmentStatus = 128;

    public const int SerialNumber = 256;
    public const int DeviceStatus = 128;
    public const int Producer = 256;

    public const int RoomNumber = 8;
    public const int Floor = 64;
    public const int Building = 64;

    public const int Gender = 16;
    public const int InsuranceStatus = 16;
    public const int Allergies = 1024;

    private const int Title = 256;
    public const int AppointmentTitle = Title;
    public const int ResultTitle = Title;

    public const int Symptoms = 4096;
    public const int Diagnosis = 4096;
    public const int Treatment = 4096;
    public const int AppointmentProtocolStatus = 128;
    
    private const int Remarks = 4096;
    public const int PatientRemarks = Remarks;
    public const int ResultRemarks = Remarks;
    public const int AppointmentProtocolRemarks = Remarks;
    
    public const int Appendix = 7 * 1024 * 1024;
    public const int AppendixContentType = 16;
}