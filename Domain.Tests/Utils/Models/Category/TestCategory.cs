using Domain.Common.Base;
using Domain.Entities.IdentityEntities;

namespace Domain.Tests.Utils.Models.Category;

public class TestCategory(
    Clinic clinic,
    string name,
    string abbreviation,
    string color) 
    : CategoryBase<TestCategoryItem>(
        clinic,
        name,
        abbreviation,
        color)
{
    // Properties
    public ICollection<TestCategoryItem> CategoryItems => Items;
    protected override string ItemsPropertyName => nameof(TestCategoryItem);
}