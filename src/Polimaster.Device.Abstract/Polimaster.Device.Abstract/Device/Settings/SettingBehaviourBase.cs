namespace Polimaster.Device.Abstract.Device.Settings;

/// <inheritdoc />
public class SettingBehaviourBase : ISettingBehaviour {

    /// <summary>
    /// SettingBehaviour constructor
    /// </summary>
    /// <param name="accessLevel">See <see cref="AccessLevel"/></param>
    /// <param name="groupName">See <see cref="GroupName"/></param>
    public SettingBehaviourBase(SettingAccessLevel accessLevel = SettingAccessLevel.BASE, string? groupName = null) {
        AccessLevel = accessLevel;
        GroupName = groupName;
    }

    /// <inheritdoc />
    public SettingAccessLevel AccessLevel { get; }

    /// <inheritdoc />
    public string? GroupName { get; }
}