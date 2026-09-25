using Domain.Entities.IdentityEntities;
using Domain.Tests.Utils.Models.Category;
using TestUtils.Builders.AuthBuilders;

namespace Domain.Tests.Utils.Builders.CategoryBuilders;

public class TestCategoryBuilder
{
    // Properties
    public Clinic? Clinic { get; private set; }
    public string? Name { get; private set; }
    public string? Abbreviation { get; private set; }
    public string? Color { get; private set; }
    
    // Constructor
    private TestCategoryBuilder(Clinic defaultClinic)
    {
        // Generating and setting default values
        Clinic = defaultClinic;
        Name = "test-name";
        Abbreviation = "test-abbreviation";
        Color = "#000000";
    }
    
    
    /* - - - Factory methods - - - */
    public static TestCategoryBuilder Create(Clinic? defaultClinic = null)
    {
        return new TestCategoryBuilder(defaultClinic ?? TestClinicBuilder.Create().Build());
    }

    public TestCategory Build()
    {
        return new TestCategory(
            Clinic!,
            Name!,
            Abbreviation!,
            Color!);
    }
    
    public TestCategoryBuilder AsUpdate()
    {
        // Setting default values
        Name = "test-updated-name";
        Abbreviation = "test-updated-abbreviation";
        Color = "#ffffff";
        
        // Returning instance
        return this;
    }
    
    public Action Apply(TestCategory testCategory) =>
        () => testCategory.Update(
            Name!,
            Abbreviation!,
            Color!);
    
    
    /* - - - Override methods - - - */
    public TestCategoryBuilder WithClinic(Clinic? clinic)
    {
        Clinic = clinic;
        return this;
    }

    public TestCategoryBuilder WithName(string? name)
    {
        Name = name;
        return this;
    }

    public TestCategoryBuilder WithAbbreviation(string? abbreviation)
    {
        Abbreviation = abbreviation;
        return this;
    }

    public TestCategoryBuilder WithColor(string? color)
    {
        Color = color;
        return this;
    }
}