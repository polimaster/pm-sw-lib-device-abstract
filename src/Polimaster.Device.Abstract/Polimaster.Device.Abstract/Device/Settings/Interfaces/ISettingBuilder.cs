using Polimaster.Device.Abstract.Device.Commands;

namespace Polimaster.Device.Abstract.Device.Settings.Interfaces;


/// <summary>
/// Device settings builder
/// </summary>
public interface ISettingBuilder {
    /// <summary>
    /// Add write command
    /// </summary>
    /// <typeparam name="T">Command value type</typeparam>
    /// <returns><see cref="ISettingBuilder"/></returns>
    ISettingBuilder WithWriteCommand<T>(IDataWriter<T> command);

    /// <summary>
    /// Add read command
    /// </summary>
    /// <typeparam name="T">Command value type</typeparam>
    /// <returns><see cref="ISettingBuilder"/></returns>
    ISettingBuilder WithReadCommand<T>(IDataReader<T> command);

    /// <summary>
    /// Define custom <see cref="IDeviceSetting{T}"/> implementation
    /// </summary>
    /// <typeparam name="T">Implementation type</typeparam>
    /// <typeparam name="TSetting">Implementation <see cref="IDeviceSetting{T}.Value"/> type</typeparam>
    /// <returns></returns>
    ISettingBuilder WithImplementation<T, TSetting>() where T : IDeviceSetting<TSetting>;
    
    /// <summary>
    /// Build setting
    /// </summary>
    /// <typeparam name="TSetting">Setting <see cref="IDeviceSetting{T}.Value"/> type</typeparam>
    /// <returns><see cref="IDeviceSetting{T}"/></returns>
    IDeviceSetting<TSetting> Build<TSetting>();

    
    /// <summary>
    /// Build setting with proxy to underlying <see cref="IDeviceSetting{T}"/>
    /// </summary>
    /// <typeparam name="T">Proxy implementation</typeparam>
    /// <typeparam name="TSetting"><see cref="IDeviceSetting{T}.Value"/> type</typeparam>
    /// <typeparam name="TProxied">Proxied <see cref="IDeviceSetting{T}"/></typeparam>
    /// <returns></returns>
    T BuildWithProxy<T, TSetting, TProxied>()
        where T : ADeviceSettingProxy<TSetting, TProxied>;
}