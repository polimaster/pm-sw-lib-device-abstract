using System;
using System.ComponentModel;
using System.Linq;

namespace Polimaster.Device.Abstract.Helpers;

/// <summary>
/// Enum helpers
/// </summary>
public static class EnumExtensions {

    /// <summary>
    /// Returns <see cref="DescriptionAttribute"/> of target enum value
    /// </summary>
    public static string? GetDescription(this Enum value) {
        var member = value.GetType().GetMember(value.ToString()).FirstOrDefault();
        var attr = member?.GetCustomAttributes(typeof(DescriptionAttribute), false)
            .Cast<DescriptionAttribute>()
            .FirstOrDefault();
        return attr?.Description;
    }

    /// <summary>
    /// Return array of enum values
    /// </summary>
    public static T[] GetValues<T>() => (T[])Enum.GetValues(typeof(T));
}