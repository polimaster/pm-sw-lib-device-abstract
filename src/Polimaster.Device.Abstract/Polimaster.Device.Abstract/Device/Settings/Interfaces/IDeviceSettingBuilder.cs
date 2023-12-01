﻿using Polimaster.Device.Abstract.Device.Commands.Interfaces;

namespace Polimaster.Device.Abstract.Device.Settings.Interfaces;


/// <summary>
/// Device settings builder
/// </summary>
public interface ISettingBuilder {
    /// <summary>
    /// Add write command
    /// </summary>
    /// <typeparam name="TCommand">Command value type</typeparam>
    /// <returns><see cref="ISettingBuilder"/></returns>
    ISettingBuilder WithWriteCommand<TCommand>(ICommand<TCommand> command);

    /// <summary>
    /// Add read command
    /// </summary>
    /// <typeparam name="TCommand">Command value type</typeparam>
    /// <returns><see cref="ISettingBuilder"/></returns>
    ISettingBuilder WithReadCommand<TCommand>(ICommand<TCommand> command);

    /// <summary>
    /// Define custom <see cref="IDeviceSetting{T}"/> implementation
    /// </summary>
    /// <typeparam name="T">Implementation type</typeparam>
    /// <typeparam name="TSetting">Implementation <see cref="IDeviceSetting{T}.Value"/> type</typeparam>
    /// <returns></returns>
    ISettingBuilder WithImplementation<T, TSetting>() where T : class, IDeviceSetting<TSetting>, new();
    
    /// <summary>
    /// Build setting
    /// </summary>
    /// <typeparam name="TSetting">Setting <see cref="IDeviceSetting{T}.Value"/> type</typeparam>
    /// <returns><see cref="IDeviceSetting{T}"/></returns>
    IDeviceSetting<TSetting> Build<TSetting>();

    
    /// <summary>
    /// Build setting with proxy to underlying <see cref="IDeviceSetting{T}"/>
    /// </summary>
    /// <typeparam name="T">Proxy implementation type</typeparam>
    /// <typeparam name="TSetting">Proxy <see cref="IDeviceSetting{T}.Value"/> type</typeparam>
    /// <typeparam name="TProxied"><see cref="ICommand{T}.Value"/> type of command</typeparam>
    /// <returns></returns>
    T BuildWithProxy<T, TSetting, TProxied>(IDeviceSetting<TProxied> proxied)
        where T : class, IDeviceSettingProxy<TSetting, TProxied>, new();
}