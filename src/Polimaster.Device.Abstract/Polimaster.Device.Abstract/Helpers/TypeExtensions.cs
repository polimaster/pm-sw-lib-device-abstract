using System;

namespace Polimaster.Device.Abstract.Helpers;

/// <summary>
///
/// </summary>
public static class TypeExtensions {
    /// <summary>
    /// Check if type is "simple":
    /// primitive, enum, string, decimal, DateTime, Guid etc.
    /// </summary>
    public static bool IsSimpleType(this Type type) {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) {
            return Nullable.GetUnderlyingType(type)!.IsSimpleType();
        }

        return type.IsPrimitive
               || type.IsEnum
               || type == typeof(string)
               || type == typeof(decimal)
               || type == typeof(DateTime)
               || type == typeof(DateTimeOffset)
               || type == typeof(TimeSpan)
               || type == typeof(Guid);
    }
}