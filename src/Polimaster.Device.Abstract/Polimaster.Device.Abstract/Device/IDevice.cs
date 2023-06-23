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
/// Device base
/// </summary>
public interface IDevice : IDisposable {
    DeviceInfo DeviceInfo { get; protected set; }
    
    /// <summary>
    /// Read device information
    /// </summary>
    /// <returns></returns>
    Task<DeviceInfo> ReadDeviceInfo();
    
    /// <summary>
    /// Indicates device is disconnected and will be removed from memory
    /// </summary>
    Action? IsDisposing { get; set; }
    
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
}

/// <summary>
/// Device can send commands
/// </summary>
/// <typeparam name="TData"><see cref="ICommand{TCompiled}"/></typeparam>
public interface IDevice<TData> : IDevice {
    
    /// <summary>
    /// Send command to device
    /// </summary>
    /// <param name="command"><see cref="ICommand{TCompiled}"/> to be send to device</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    Task SendCommand(ICommand<TData> command, CancellationToken cancellationToken = new());
    

    /// <summary>
    /// Reads data from device with command
    /// </summary>
    /// <param name="command"><see cref="IResultCommand{TParseResult,TCompiled}"/> to be send to device
    /// </param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <typeparam name="TParseResult">Result of <see cref="IResultCommand{TParseResult,TCompiled}"/></typeparam>
    Task<TParseResult?> SendCommand<TParseResult>(IResultCommand<TParseResult, TData> command, CancellationToken cancellationToken = new());
}


/// <summary>
/// Device with identified <see cref="Transport"/>
/// </summary>
/// <typeparam name="TData">Command value type <see cref="ICommand{TParam,TData}"/></typeparam>
/// <typeparam name="TConnectionParams">Connection parameters type for <see cref="ITransport{TData,TConnectionParams}"/></typeparam>
public interface IDevice<TData, TConnectionParams> : IDevice<TData> {

    /// <summary>
    /// Transport layer
    /// </summary>
    /// <see cref="ITransport{TData, TConnectionParams}"/>
    ITransport<TData, TConnectionParams?> Transport { get; }
}