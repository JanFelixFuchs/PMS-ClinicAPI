using Domain.Common.Enums;
using Domain.Entities.IdentityEntities;
using TestUtils.Constants;

namespace TestUtils.Builders.AuthBuilders;

public class TestClinicBuilder
{
    // Properties
    public string? Code { get; private set; }
    public string? Name { get; private set; }
    public string? Abbreviation { get; private set; }
    public string? Owner { get; private set; }
    public MedicalField MedicalField { get; private set; }
    public string? Street { get; private set; }
    public string? HouseNumber { get; private set; }
    public string? City { get; private set; }
    public string? ZipCode { get; private set; }
    public Country Country { get; private set; }
    public string? Email { get; private set; }
    public string? PhoneNumber { get; private set; }
    
    // Constructor
    private TestClinicBuilder()
    {
        // Setting default values
        Code = TestConstants.ValidInitialCode;
        Name = TestConstants.ValidInitialClinicName;
        Abbreviation = TestConstants.ValidInitialAbbreviation;
        Owner = TestConstants.ValidInitialOwner;
        MedicalField = TestConstants.ValidInitialMedicalField;
        Street = TestConstants.ValidInitialStreet;
        HouseNumber = TestConstants.ValidInitialHouseNumber;
        City = TestConstants.ValidInitialCity;
        ZipCode = TestConstants.ValidInitialZipCode;
        Country = TestConstants.ValidInitialCountry;
        Email = TestConstants.ValidInitialEmail;
        PhoneNumber = TestConstants.ValidInitialPhoneNumber;
    }
    
    
    /* - - - Factory methods - - - */
    public static TestClinicBuilder Create() => new();

    public Clinic Build()
    {
        return new Clinic(
            Code!,
            Name!,
            Abbreviation!,
            Owner!,
            MedicalField,
            Street!,
            HouseNumber!,
            City!,
            ZipCode!,
            Country,
            Email!,
            PhoneNumber!);
    }
    
    public TestClinicBuilder AsUpdate()
    {
        // Setting default values
        Name = TestConstants.ValidUpdatedClinicName;
        Abbreviation = TestConstants.ValidUpdatedAbbreviation;
        Owner = TestConstants.ValidUpdatedOwner;
        MedicalField = TestConstants.ValidUpdatedMedicalField;
        Street = TestConstants.ValidUpdatedStreet;
        HouseNumber = TestConstants.ValidUpdatedHouseNumber;
        City = TestConstants.ValidUpdatedCity;
        ZipCode = TestConstants.ValidUpdatedZipCode;
        Country = TestConstants.ValidUpdatedCountry;
        Email = TestConstants.ValidUpdatedEmail;
        PhoneNumber = TestConstants.ValidUpdatedPhoneNumber;
        
        // Returning instance
        return this;
    }
    
    public Action Apply(Clinic clinic) =>
        () => clinic.Update(
            Name!,
            Abbreviation!,
            Owner!,
            MedicalField,
            Street!,
            HouseNumber!,
            City!,
            ZipCode!,
            Country,
            Email!,
            PhoneNumber!);
    
    
    /* - - - Override methods - - - */
    public TestClinicBuilder WithCode(string? code)
    {
        Code = code;
        return this;
    }

    public TestClinicBuilder WithName(string? name)
    {
        Name = name;
        return this;
    }

    public TestClinicBuilder WithAbbreviation(string? abbreviation)
    {
        Abbreviation = abbreviation;
        return this;
    }

    public TestClinicBuilder WithOwner(string? owner)
    {
        Owner = owner;
        return this;
    }

    public TestClinicBuilder WithMedicalField(MedicalField medicalField)
    {
        MedicalField = medicalField;
        return this;
    }

    public TestClinicBuilder WithStreet(string? street)
    {
        Street = street;
        return this;
    }

    public TestClinicBuilder WithHouseNumber(string? houseNumber)
    {
        HouseNumber = houseNumber;
        return this;
    }
    
    public TestClinicBuilder WithCity(string? city)
    {
        City = city;
        return this;
    }
    
    public TestClinicBuilder WithZipCode(string? zipCode)
    {
        ZipCode = zipCode;
        return this;
    }
    
    public TestClinicBuilder WithCountry(Country country)
    {
        Country = country;
        return this;
    }

    public TestClinicBuilder WithEmail(string? email)
    {
        Email = email;
        return this;
    }

    public TestClinicBuilder WithPhoneNumber(string? phoneNumber)
    {
        PhoneNumber = phoneNumber;
        return this;
    }
}