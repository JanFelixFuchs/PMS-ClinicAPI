using System.Reflection;
using Domain.Common.Interfaces;

namespace TestUtils.Helper;

public static class TestEntityHelper
{
    public static void SetId<T>(T entity, Guid id) where T : IEntity
    {
        typeof(T)
            .GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!
            .SetValue(entity, id);
    }
}