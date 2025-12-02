using System;
using System.ComponentModel.DataAnnotations;

namespace Polimaster.Device.Abstract.Tests.Impl.Settings;

/// <inheritdoc />
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class TimeSpanRangeAttribute(long minSeconds, long maxSeconds) : ValidationAttribute {
    /// <summary>
    /// Value in seconds
    /// </summary>
    public TimeSpan Min { get; } = TimeSpan.FromSeconds(minSeconds);

    /// <summary>
    /// Value in seconds
    /// </summary>
    public TimeSpan Max { get; } = TimeSpan.FromSeconds(maxSeconds);

    /// <inheritdoc />
    public override bool IsValid(object? value) {
        return value switch {
            null => true,
            TimeSpan ts => ts >= Min && ts <= Max,
            _ => false
        };
    }

    /// <inheritdoc />
    public override string FormatErrorMessage(string name) {
        return ErrorMessage ?? $"{name} shall be between {Min.TotalHours:00}:{Min.Minutes:00} and {Max.TotalHours:00}:{Max.Minutes:00}.";
    }
}