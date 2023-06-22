using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Commands;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device;

/// <summary>
/// Device
/// </summary>
/// <typeparam name="TData">Command value type <see cref="ICommand{TParam,TData}"/></typeparam>
/// <typeparam name="TConnectionParams">Connection parameters type</typeparam>
public interface IDevice<TData, TConnectionParams> : IDisposable {
    
    DeviceInfo DeviceInfo { get; protected set; }
    
    /// <summary>
    /// Read device information
    /// </summary>
    /// <returns></returns>
    Task<DeviceInfo> ReadDeviceInfo();

    /// <summary>
    /// Transport layer
    /// </summary>
    /// <see cref="ITransport{TData, TConnectionParams}"/>
    ITransport<TData, TConnectionParams?> Transport { get; }

    /// <summary>
    /// Reads device settings
    /// </summary>
    /// <returns></returns>
    Task ReadSettings();

    /// <summary>
    /// Writes settings to device
    /// </summary>
    /// <returns></returns>
    Task WriteSettings();

    /// <summary>
    /// Search for <see cref="IDeviceSetting{T}"/> properties in device object
    /// </summary>
    /// <returns>Array of <see cref="PropertyInfo"/></returns>
    IEnumerable<PropertyInfo> GetDeviceSettingsProperties();

    /// <summary>
    /// Send command to device
    /// </summary>
    /// <param name="command">
    ///     <see cref="ICommand{TParam,TData}"/>
    ///     Command to be send to device</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <typeparam name="TParam"><see cref="ICommand{TParam,TData}"/></typeparam>
    Task SendCommand<TParam>(ICommand<TParam, TData> command, CancellationToken cancellationToken = new());
    

    /// <summary>
    /// Reads data from device with command
    /// </summary>
    /// <param name="command">
    ///     <see cref="IResultCommand{TResult,TParam,TData}"/>
    ///     Command to be send to device
    /// </param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <typeparam name="TResult"><see cref="IResultCommand{TResult,TParam,TData}"/></typeparam>
    /// <typeparam name="TParam"><see cref="IResultCommand{TResult,TParam,TData}"/></typeparam>
    Task<TResult?> SendCommand<TResult, TParam>(IResultCommand<TResult, TParam, TData> command, CancellationToken cancellationToken = new());
}