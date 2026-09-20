using Domain.Common.Enums;
using Domain.Entities.DeviceEntities;
using Domain.Entities.IdentityEntities;
using TestUtils.Builders.AuthBuilders;
using TestUtils.Constants;

namespace TestUtils.Builders.DeviceBuilders;

public class TestDeviceBuilder
{
    // Properties
    public Clinic? Clinic;
    public string? Name;
    public string? Abbreviation;
    public string? SerialNumber;
    public DeviceStatus Status;
    public string? Producer;
    public ICollection<DeviceCategory>? DeviceCategories;
    public DateTime? DateOfPurchase;
    public DateTime? DateOfLastMaintenance;
    public readonly DateTime CreationDateTime;
    private DateTime _updateDateTime;
    
    // Constructor
    private TestDeviceBuilder(
        Clinic defaultClinic,
        DateTime creationDateTime)
    {
        // Setting default values
        Clinic = defaultClinic;
        Name = "test-name";
        Abbreviation = "test-abbreviation";
        SerialNumber = "test-serial-number";
        Status = DeviceStatus.Operational;
        Producer = "test-producer";
        DeviceCategories = new List<DeviceCategory>{ TestDeviceCategoryBuilder.Create(defaultClinic).Build() };
        DateOfPurchase = creationDateTime;
        DateOfLastMaintenance = creationDateTime;
        CreationDateTime = creationDateTime;
        _updateDateTime = creationDateTime.AddHours(1);
    }
    
    
    /* - - - Factory methods - - - */
    public static TestDeviceBuilder Create(
        Clinic? defaultClinic = null,
        DateTime? creationDateTime = null)
    {
        return new TestDeviceBuilder(
            defaultClinic ?? TestClinicBuilder.Create().Build(),
            creationDateTime ?? TestConstants.DefaultCurrentDateTime);
    }

    public Device Build()
    {
        return new Device(
            Clinic!,
            Name!,
            Abbreviation!,
            SerialNumber!,
            Status,
            Producer!,
            DeviceCategories!,
            DateOfPurchase,
            DateOfLastMaintenance,
            CreationDateTime);
    }

    public TestDeviceBuilder AsUpdate(DateTime? updateDateTime = null)
    {
        // Setting custom update time if provided
        if (updateDateTime.HasValue)
            _updateDateTime = updateDateTime.Value;
        
        // Setting default values
        Name = "test-updated-name";
        Abbreviation = "test-updated-abbreviation";
        DeviceCategories = new List<DeviceCategory>{ TestDeviceCategoryBuilder.Create(Clinic).Build() };
        DateOfLastMaintenance = _updateDateTime;
        
        // Returning instance
        return this;
    }

    public Action Apply(Device device) =>
        () => device.Update(
            Name!,
            Abbreviation!,
            DeviceCategories!,
            DateOfLastMaintenance,
            _updateDateTime);
    
        
    /* - - - Override methods - - - */
    public TestDeviceBuilder WithClinic(Clinic? clinic)
    {
        Clinic = clinic;
        return this;
    }

    public TestDeviceBuilder WithName(string? name)
    {
        Name = name;
        return this;
    }

    public TestDeviceBuilder WithAbbreviation(string? abbreviation)
    {
        Abbreviation = abbreviation;
        return this;
    }

    public TestDeviceBuilder WithSerialNumber(string? serialNumber)
    {
        SerialNumber = serialNumber;
        return this;
    }

    public TestDeviceBuilder WithStatus(DeviceStatus status)
    {
        Status = status;
        return this;
    }

    public TestDeviceBuilder WithProducer(string? producer)
    {
        Producer = producer;
        return this;
    }

    public TestDeviceBuilder WithDeviceCategories(ICollection<DeviceCategory>? deviceCategories)
    {
        DeviceCategories = deviceCategories;
        return this;
    }

    public TestDeviceBuilder WithDateOfPurchase(DateTime? dateOfPurchase)
    {
        DateOfPurchase = dateOfPurchase;
        return this;
    }

    public TestDeviceBuilder WithDateOfLastMaintenance(DateTime? dateOfLastMaintenance)
    {
        DateOfLastMaintenance = dateOfLastMaintenance;
        return this;
    }
}