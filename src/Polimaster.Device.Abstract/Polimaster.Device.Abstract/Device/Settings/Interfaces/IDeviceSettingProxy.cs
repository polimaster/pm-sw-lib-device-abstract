namespace Polimaster.Device.Abstract.Device.Settings.Interfaces;


/// <summary>
/// Proxied device setting. Converts underlying <see cref="IDeviceSetting{T}"/> value to its own.
/// Usually, its required when device returns structured value like byte masks or complex strings.
/// </summary>
/// <typeparam name="T"><inheritdoc cref="IDeviceSetting{T}"/></typeparam>
/// <typeparam name="TProxied">Proxied <see cref="IDeviceSetting{T}"/> value type</typeparam>
public interface IDeviceSettingProxy<T, TProxied> : IDeviceSetting<T> {
    
    /// <summary>
    /// Proxied <see cref="IDeviceSetting{T}"/> 
    /// </summary>
    IDeviceSetting<TProxied>? ProxiedSetting { get; set; }
    
    /// <summary>
    /// Converts <see cref="ProxiedSetting"/> value to <see cref="IDeviceSetting{T}.Value"/>
    /// </summary>
    /// <param name="value"><see cref="ProxiedSetting"/> value</param>
    /// <returns>Result of conversion</returns>
    T FromProxied(TProxied? value);
    
    /// <summary>
    /// Converts <see cref="IDeviceSetting{T}.Value"/> to <see cref="ProxiedSetting"/> value
    /// </summary>
    /// <param name="value"><see cref="IDeviceSetting{T}.Value"/></param>
    /// <returns>Result of conversion</returns>
    TProxied FromCommand(T? value);
}