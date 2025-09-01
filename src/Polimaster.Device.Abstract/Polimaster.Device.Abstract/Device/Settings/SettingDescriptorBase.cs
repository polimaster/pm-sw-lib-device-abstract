namespace Polimaster.Device.Abstract.Device.Settings;

/// <inheritdoc />
public class SettingDescriptorBase(string name, SettingAccessLevel accessLevel = SettingAccessLevel.BASE, string? groupName = null, string? description = null) : ISettingDescriptor {
    /// <inheritdoc />
    public SettingAccessLevel AccessLevel { get; set; } = accessLevel;

    /// <inheritdoc />
    public string Name { get; init; } = name;

    /// <inheritdoc />
    public string? Description { get; init; } = description;

    /// <inheritdoc />
    public string? GroupName { get; init; } = groupName;

    /// <inheritdoc />
    public override int GetHashCode() {
        unchecked { return (Name.GetHashCode() * 397) ^ (GroupName != null ? GroupName.GetHashCode() : 0); }
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    private bool Equals(SettingDescriptorBase other) {
        return Name == other.Name && GroupName == other.GroupName;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((SettingDescriptorBase)obj);
    }

    /// <summary>
    /// == operator
    /// </summary>
    public static bool operator ==(SettingDescriptorBase? left, SettingDescriptorBase? right) {
        return Equals(left, right);
    }

    /// <summary>
    /// != operator
    /// </summary>
    public static bool operator !=(SettingDescriptorBase? left, SettingDescriptorBase? right) {
        return !Equals(left, right);
    }
}