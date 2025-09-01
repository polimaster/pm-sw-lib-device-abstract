namespace Polimaster.Device.Abstract.Device.Settings;

/// <inheritdoc />
public class SettingDescriptorBase : ISettingDescriptor {
    /// <inheritdoc />
    public SettingAccessLevel AccessLevel { get; set; }

    /// <inheritdoc />
    public string? Name { get; init; }

    /// <inheritdoc />
    public string? Description { get; init; }

    /// <inheritdoc />
    public string? GroupName { get; init; }

    /// <inheritdoc />
    public override int GetHashCode() {
        unchecked { return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (GroupName != null ? GroupName.GetHashCode() : 0); }
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