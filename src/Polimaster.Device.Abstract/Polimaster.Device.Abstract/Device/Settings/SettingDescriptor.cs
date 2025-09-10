using System;
using System.Collections.Generic;

namespace Polimaster.Device.Abstract.Device.Settings;

/// <inheritdoc />
public class SettingDescriptor(
    string name,
    Type valueType,
    SettingAccessLevel accessLevel = SettingAccessLevel.BASE,
    string? groupName = null,
    string? description = null,
    object? unit = null,
    IEnumerable<object>? valueList = null) : ISettingDescriptor {
    /// <inheritdoc />
    public SettingAccessLevel AccessLevel { get; set; } = accessLevel;

    /// <inheritdoc />
    public Type ValueType { get; } = valueType;

    /// <inheritdoc />
    public string Name { get; } = name;

    /// <inheritdoc />
    public string? Description { get; } = description;

    /// <inheritdoc />
    public string? GroupName { get; } = groupName;

    /// <inheritdoc />
    public object? Unit { get; } = unit;

    /// <inheritdoc />
    public IEnumerable<object>? ValueList { get; } = valueList;

    /// <inheritdoc />
    public override int GetHashCode() {
        unchecked {
            var hash = 17;
            hash = hash * 23 + Name.GetHashCode();
            hash = hash * 23 + (GroupName?.GetHashCode() ?? 0);
            hash = hash * 23 + ValueType.GetHashCode();
            return hash;
        }
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    private bool Equals(SettingDescriptor other) {
        return Name == other.Name && GroupName == other.GroupName;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((SettingDescriptor)obj);
    }

    /// <summary>
    /// == operator
    /// </summary>
    public static bool operator ==(SettingDescriptor? left, SettingDescriptor? right) {
        return Equals(left, right);
    }

    /// <summary>
    /// != operator
    /// </summary>
    public static bool operator !=(SettingDescriptor? left, SettingDescriptor? right) {
        return !Equals(left, right);
    }
}