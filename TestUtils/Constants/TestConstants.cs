using Domain.Common.Enums;
using Xunit;

namespace TestUtils.Constants;

public static class TestConstants
{
    /* - - - General constants - - - */
    // Default current date time
    public static readonly DateTime DefaultCurrentDateTime = new(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);
    
    // Placeholder for length tests
    public const char LengthTestPlaceholder = '*';
    
    // Country-specific data
    private static readonly Dictionary<Country, (string ZipCode, string PhoneNumberPrefix, string PhoneNumber)> CountrySpecificData = new()
    {
        { Country.De, ("12345", "+49", "+49123456789")},
        { Country.Fi, ("12345", "+358", "+358123456789")}
    };
    public static TheoryData<string, string, Country> ValidCountryRelatedInformation =>
    [
        .. CountrySpecificData.Select(countryData => 
            (countryData.Value.ZipCode, countryData.Value.PhoneNumber, countryData.Key))
    ];
    
    
    /* - - - Valid values - - - */
    // General: Abbreviation
    public const string ValidInitialAbbreviation = "test-abbreviation";
    public const string ValidUpdatedAbbreviation = "test-updated-abbreviation";
    
    // Category: Name
    public const string ValidInitialCategoryName = "test-category-name";
    public const string ValidUpdatedCategoryName = "test-updated-category-name";
    
    // Category: Color
    public const string ValidInitialColor = "#000000";
    public const string ValidUpdatedColor = "#ffffff";
    
    // AppointmentProtocol: Symptoms
    public const string ValidUpdatedSymptoms = "test-updated-symptoms";

    // AppointmentProtocol: Diagnosis
    public const string ValidUpdatedDiagnosis = "test-updated-diagnosis";
    
    // AppointmentProtocol: Treatment
    public const string ValidUpdatedTreatment = "test-updated-treatment";
    
    // AppointmentProtocol: Remarks
    public const string ValidUpdatedAppointmentProtocolRemarks = "test-updated-appointment-protocol-remarks";
    
    // Appointment: Title
    public const string ValidInitialAppointmentTitle = "test-appointment-title";
    public const string ValidUpdatedAppointmentTitle = "test-updated-appointment-title";
    
    // Result: Title
    public const string ValidInitialResultTitle = "test-result-title";
    
    // Result: Appendix
    public static readonly byte[] ValidInitialAppendix = [0x25, 0x50, 0x44, 0x46, 0x00, 0x01, 0x02, 0x03, 0x04];
    public static TheoryData<AppendixContentType, byte[]> ValidAppendixContentTypes => new()
    {
        { AppendixContentType.Pdf, [0x25, 0x50, 0x44, 0x46] },
        { AppendixContentType.Jpeg, [0xFF, 0xD8, 0xFF] },
        { AppendixContentType.Png, [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A] }
    };
        
    // Result: Remarks
    public const string ValidInitialResultRemarks = "test-result-remarks";
    
    // Claim: Type
    public const ClaimType ValidInitialClaimType = ClaimType.Appointment;
    
    // Claim: Value
    public const ClaimValue ValidInitialClaimValue = ClaimValue.None;
    
    // Clinic: Code
    public const string ValidInitialCode = "TESTCODE";
    public const string ValidUpdatedCode = "TESTUPDATEDCODE";
    
    // Clinic: Name
    public const string ValidInitialClinicName  = "test-clinic-name";
    public const string ValidUpdatedClinicName = "test-updated-clinic-name";
    
    // Clinic: Owner
    public const string ValidInitialOwner = "test-owner";
    public const string ValidUpdatedOwner = "test-updated-owner";
    
    // Clinic: Medical Field
    public const MedicalField ValidInitialMedicalField = MedicalField.GeneralMedicine;
    public const MedicalField ValidUpdatedMedicalField = MedicalField.InternalMedicine;
    
    // Role: Name
    public const string ValidInitialRoleName = "test-role-name";
    public const string ValidUpdatedRoleName = "test-updated-role-name";
    
    // User: Username
    public const string ValidInitialUsername = "test_username";
    public const string ValidUpdatedUsername = "test_updated_username";
    
    // User: Password
    public const string ValidInitialPassword = "TestPassword1!";
    public const string ValidUpdatedPassword = "TestUpdatedPassword1!";
    
    // User: PasswordHash
    public const string ValidInitialPasswordHash = "test-password-hash";
    public const string ValidUpdatedPasswordHash = "test-updated-password-hash";
    
    // User: Refresh Token
    public const string ValidInitialRefreshToken = "test-refresh-token";
    
    // User: Refresh Token Hash
    public const string ValidUpdatedRefreshTokenHash = "test-updated-refresh-token-hash";
    
    // Clinician: First Name
    public const string ValidInitialClinicianFirstName = "test-clinician-first-name";
    
    // Clinician: Last Name
    public const string ValidInitialClinicianLastName = "test-clinician-last-name";
    public const string ValidUpdatedClinicianLastName = "test-updated-clinician-last-name";
    
    // Device: Name
    public const string ValidInitialDeviceName = "test-device-name";
    public const string ValidUpdatedDeviceName = "test-updated-device-name";
    
    // Device: Serial Number
    public const string ValidInitialSerialNumber = "test-serial-number";
    
    // Device: Status
    public const DeviceStatus ValidInitialDeviceStatus = DeviceStatus.Operational;
    public const DeviceStatus ValidUpdatedDeviceStatus = DeviceStatus.InMaintenance;
    
    // Device: Producer
    public const string ValidInitialProducer = "test-producer";
    
    // Patient: First Name
    public const string ValidInitialPatientFirstName = "test-patient-first-name";
    
    // Patient: Last Name
    public const string ValidInitialPatientLastName = "test-patient-last-name";
    public const string ValidUpdatedPatientLastName = "test-updated-patient-last-name";
    
    // Patient: Gender
    public const Gender ValidInitialGender = Gender.Male;
    
    // Patient: Insurance Status
    public const InsuranceStatus ValidInitialInsuranceStatus = InsuranceStatus.Statutory;
    public const InsuranceStatus ValidUpdatedInsuranceStatus = InsuranceStatus.Private;
    
    // Patient: Allergies
    public const string ValidInitialAllergies = "test-allergies";
    public const string ValidUpdatedAllergies = "test-updated-allergies";
    
    // Patient: Remarks
    public const string ValidInitialPatientRemarks = "test-patient-remarks";
    public const string ValidUpdatedPatientRemarks = "test-updated-patient-remarks";
    
    // Room: Name
    public const string ValidInitialRoomName = "test-room-name";
    public const string ValidUpdatedRoomName = "test-updated-room-name";
    
    // Room: Number
    public const string ValidInitialRoomNumber = "123";
    public const string ValidUpdatedRoomNumber = "456";
    
    // Room: Floor
    public const string ValidInitialFloor = "test-floor";
    public const string ValidUpdatedFloor = "test-updated-floor";
    
    // Room: Building
    public const string ValidInitialBuilding = "test-building";
    public const string ValidUpdatedBuilding = "test-updated-building";
    
    // Address: Street
    public const string ValidInitialStreet = "test-street";
    public const string ValidUpdatedStreet = "test-updated-street";
    
    // Address: House Number
    public const string ValidInitialHouseNumber = "123";
    public const string ValidUpdatedHouseNumber = "456";
    
    // Address: City
    public const string ValidInitialCity = "test-city";
    public const string ValidUpdatedCity = "test-updated-city";
    
    // Address: Zip Code
    public static readonly string ValidInitialZipCode = CountrySpecificData[Country.De].ZipCode;
    public static readonly string ValidUpdatedZipCode = CountrySpecificData[Country.Fi].ZipCode;
    public static TheoryData<string, Country> ValidZipCodes =>
    [
        .. CountrySpecificData.Select(countryData => (countryData.Value.ZipCode, countryData.Key))
    ];

    // Address: Country
    public const Country ValidInitialCountry = Country.De;
    public const Country ValidUpdatedCountry = Country.Fi;
    
    // Contact Information: Email
    public const string ValidInitialEmail = "test@email.com";
    public const string ValidUpdatedEmail = "test-updated@email.com";
    
    // Contact Information: Phone Number
    public static readonly string ValidInitialPhoneNumber = CountrySpecificData[Country.De].PhoneNumber;
    public static readonly string ValidUpdatedPhoneNumber = CountrySpecificData[Country.Fi].PhoneNumber;
    public static TheoryData<string, Country> ValidPhoneNumberPrefixes =>
    [
        .. CountrySpecificData.Select(countryData => (countryData.Value.PhoneNumberPrefix, countryData.Key))
    ];
    public static TheoryData<string, Country> ValidPhoneNumbers =>
    [
        .. CountrySpecificData.Select(countryData => (countryData.Value.PhoneNumber, countryData.Key))
    ];
    

    /* - - - Invalid values - - - */
    // General
    public static TheoryData<string?> InvalidNullEmptyOrWhitespaceString => 
    [
        null!,
        "",
        "   ",
    ];
    
    public static TheoryData<string> InvalidEmptyOrWhitespaceString => 
    [
        "",
        "   ",
    ];
    
    public static TheoryData<string, Country> InvalidZipCodesNotMatchingRegex =>
    [
        ("1234", Country.De),
        ("123456", Country.De),
        ("1234A", Country.De),
        ("1234", Country.Fi),
        ("123456", Country.Fi),
        ("1234A", Country.Fi),
    ];
    
    public static TheoryData<string> InvalidEmailsNotMatchingRegex =>
    [
        "@test-missing-local-part.test",
        "test-missing-at-sign.test",
        "test@test-missing-domain",
    ];
    
    public static TheoryData<string, Country> InvalidPhoneNumbersNotMatchingRegex =>
    [
        ("0123456789", Country.De),
        ("+4901234", Country.De),
        ("+4901234567891011", Country.De),
        ("+49 01234 56789", Country.De),
        ("+49-01234-56789", Country.De),
        ("490123456789", Country.Fi),
        ("0123456789", Country.Fi),
        ("+35801234", Country.Fi),
        ("+358012345678910", Country.Fi),
        ("+358 01234 56789", Country.Fi),
        ("+358-01234-56789", Country.Fi),
        ("3580123456789", Country.Fi),
    ];
    
    public static TheoryData<string> InvalidColorsNotMatchingRegex =>
    [
        "000",
        "000000",
        "#00",
        "#0000",
        "#0000000",
        "#GGG",
        "#GGGGGG"
    ];

    public static TheoryData<string> InvalidPasswordsNotMatchingRegex =>
    [
        "",
        "   ",
        "Short1!",
        "testpasswordwithnouppercaseletter1!",
        "TESTPASSWORDWITHNOLOWERCASELETTER1!",
        "TestPasswordWithNoDigit!",
        "TestPasswordWithNoSpecialCharacter1",
        "TestPasswordWithUnallowedCharacter¡"
    ];
    
    public static TheoryData<string> InvalidUsernamesNotMatchingRegex =>
    [
        new string('A', 7),
        new string('A', 65),
        "test-username",
        "test+username",
        "test!username",
        "test#username",
        "test@username",
        "test username"
    ];
    
    public static TheoryData<string> InvalidCodesNotMatchingRegex =>
    [
        new string('A', 7),
        new string('A', 65),
        "abcdefgh",
        "ABCDEFG!",
        "ABCD EFGH"
    ];
}