using System.Reflection;

namespace TestUtils.Helper;

public static class ReflectionEntityHelper
{
    public static void SetPropertyWithGetterOnly<T, TValue>(T entity, string propertyName, TValue value)
    {
        // Determining backing field
        var backingFieldName = $"<{propertyName}>k__BackingField";
        var field = typeof(T)
            .GetField(backingFieldName, BindingFlags.Instance | BindingFlags.NonPublic) 
            ?? throw new MissingMemberException(typeof(T).FullName, backingFieldName);

        // Setting value
        field.SetValue(entity, value);
    }
}