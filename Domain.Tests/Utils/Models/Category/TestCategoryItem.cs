using Domain.Common.Interfaces;

namespace Domain.Tests.Utils.Models.Category;

public class TestCategoryItem : IEntity
{
    public Guid Id { get; } = Guid.NewGuid();
}