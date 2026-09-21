using Domain.Common.Enums;
using Domain.Entities.PatientEntities;
using Domain.Entities.IdentityEntities;
using TestUtils.Builders.AuthBuilders;
using TestUtils.Constants;

namespace TestUtils.Builders.PatientBuilders;

public class TestPatientBuilder
{
    // Properties
    public Clinic? Clinic { get; private set; }
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public Gender Gender { get; private set; }
    public string? Street { get; private set; }
    public string? HouseNumber { get; private set; }
    public string? City { get; private set; }
    public string? ZipCode { get; private set; }
    public Country Country { get; private set; }
    public string? Email { get; private set; }
    public string? PhoneNumber { get; private set; }
    public InsuranceStatus InsuranceStatus { get; private set; }
    public string? Allergies { get; private set; }
    public string? Remarks { get; private set; }
    public readonly DateTime CreationDateTime;
    
    // Constructor
    private TestPatientBuilder(
        Clinic defaultClinic,
        DateTime creationDateTime)
    {
        // Setting default values
        Clinic = defaultClinic;
        FirstName = "test-first-name";
        LastName = "test-last-name";
        DateOfBirth = creationDateTime.AddYears(-20);
        Gender = Gender.Male;
        Street = "test-street";
        HouseNumber = "123";
        City = "test-city";
        ZipCode = TestConstants.ValidGermanZipCode;
        Country = Country.De;
        Email = "test@email.com";
        PhoneNumber = TestConstants.ValidGermanPhoneNumber;
        InsuranceStatus = InsuranceStatus.Statutory;
        Allergies = "test-allergies";
        Remarks = "test-remarks";
        CreationDateTime = creationDateTime;
    }
    
    
    /* - - -  Factory methods - - - */
    public static TestPatientBuilder Create(
        Clinic? defaultClinic = null,
        DateTime? currentDateTime = null)
    {
        return new TestPatientBuilder(
            defaultClinic ?? TestClinicBuilder.Create().Build(),
            currentDateTime ?? TestConstants.DefaultCurrentDateTime);
    }
    
    public Patient Build()
    {
        return new Patient(
            Clinic!,
            FirstName!,
            LastName!,
            DateOfBirth,
            Gender,
            Street!,
            HouseNumber!,
            City!,
            ZipCode!,
            Country,
            Email!,
            PhoneNumber!,
            InsuranceStatus,
            Allergies,
            Remarks,
            CreationDateTime);
    }

    public TestPatientBuilder AsUpdate()
    {
        // Setting default values
        LastName = "test-updated-last-name";
        Street = "test-updated-street";
        HouseNumber = "456";
        City = "test-updated-city";
        ZipCode = TestConstants.ValidFinnishZipCode;
        Country = Country.Fi;
        Email = "test-updated@email.com";
        PhoneNumber = TestConstants.ValidFinnishPhoneNumber;
        InsuranceStatus = InsuranceStatus.Private;
        Allergies = "test-updated-allergies";
        Remarks = "test-updated-remarks";
        
        // Returning instance
        return this;
    }

    public Action Apply(Patient patient) =>
        () => patient.Update(
            LastName!,
            Street!,
            HouseNumber!,
            City!,
            ZipCode!,
            Country,
            Email!,
            PhoneNumber!,
            InsuranceStatus,
            Allergies,
            Remarks);
    
    
    /* - - - Override methods - - - */
    public TestPatientBuilder WithClinic(Clinic? clinic)
    {
        Clinic = clinic;
        return this;
    }

    public TestPatientBuilder WithFirstName(string? firstName)
    {
        FirstName = firstName;
        return this;
    }

    public TestPatientBuilder WithLastName(string? lastName)
    {
        LastName = lastName;
        return this;
    }

    public TestPatientBuilder WithDateOfBirth(DateTime dateOfBirth)
    {
        DateOfBirth = dateOfBirth;
        return this;
    }

    public TestPatientBuilder WithGender(Gender gender)
    {
        Gender = gender;
        return this;
    }

    public TestPatientBuilder WithStreet(string? street)
    {
        Street = street;
        return this;
    }

    public TestPatientBuilder WithHouseNumber(string? houseNumber)
    {
        HouseNumber = houseNumber;
        return this;
    }

    public TestPatientBuilder WithCity(string? city)
    {
        City = city;
        return this;
    }

    public TestPatientBuilder WithZipCode(string? zipCode)
    {
        ZipCode = zipCode;
        return this;
    }

    public TestPatientBuilder WithCountry(Country country)
    {
        Country = country;
        return this;
    }

    public TestPatientBuilder WithEmail(string? email)
    {
        Email = email;
        return this;
    }

    public TestPatientBuilder WithPhoneNumber(string? phoneNumber)
    {
        PhoneNumber = phoneNumber;
        return this;
    }

    public TestPatientBuilder WithInsuranceStatus(InsuranceStatus insuranceStatus)
    {
        InsuranceStatus = insuranceStatus;
        return this;
    }

    public TestPatientBuilder WithAllergies(string? allergies)
    {
        Allergies = allergies;
        return this;
    }

    public TestPatientBuilder WithRemarks(string? remarks)
    {
        Remarks = remarks;
        return this;
    }
}
