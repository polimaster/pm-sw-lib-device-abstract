using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Commands;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device;

/// <inheritdoc cref="IDevice{TData, TConnectionParams}"/>
public abstract class ADevice<TData, TConnectionParams> : IDevice<TData, TConnectionParams> {
    protected readonly ILogger<IDevice<TData, TConnectionParams>>? Logger;

    public DeviceInfo DeviceInfo { get; set; }
    public abstract Task<DeviceInfo> ReadDeviceInfo();

    /// <inheritdoc cref="IDevice{TData,TConnectionParams}.Transport"/>
    public virtual ITransport<TData, TConnectionParams?> Transport { get; }

    /// <summary>
    /// Device constructor
    /// </summary>
    /// <param name="transport">
    ///     <see cref="Transport"/>
    /// </param>
    /// <param name="logger"></param>
    protected ADevice(ITransport<TData, TConnectionParams?> transport,
        ILogger<ADevice<TData, TConnectionParams>>? logger = null) {
        Logger = logger;
        Transport = transport;
    }

    /// <inheritdoc cref="IDevice{TData,TConnectionParams}.Write{TParam}"/>
    public virtual async Task Write<TParam>(ICommand<TParam, TData> command,
        CancellationToken cancellationToken = new()) {
        try {
            command.Validate();
            Logger?.LogDebug("Executing command {C}", nameof(command.GetType));
            var stream = await Transport.Open();
            if (stream == null) throw new NullReferenceException("Transport stream is null");
            await Transport.Write(stream, command.Compile(), cancellationToken);
        } catch (CommandValidationException) { throw; } catch (Exception e) { throw new DeviceException(e); }
    }

    /// <inheritdoc cref="IDevice{TData,TConnectionParams}.Read{TResult,TParam}"/>
    public virtual async Task<TResult?> Read<TResult, TParam>(IResultCommand<TResult, TParam, TData> command,
        CancellationToken cancellationToken = new()) {
        try {
            command.Validate();
            Logger?.LogDebug("Executing command {C}", command.GetType().Name);
            var stream = await Transport.Open();
            if (stream == null) throw new NullReferenceException("Transport stream is null");
            var res = await Transport.Read(stream, command.Compile(), cancellationToken);
            return command.Parse(res);
        } catch (CommandValidationException) { throw; } catch (CommandResultParsingException) { throw; } catch
            (Exception e) { throw new DeviceException(e); }
    }

    public void Dispose() {
        Transport.Dispose();
    }

    /// <summary>
    /// Successor class should have properties of type <see cref="IDeviceSetting{T}"/> interface.
    /// Method iterates thru this properties and call <see cref="IDeviceSetting{T}.Read()"/> on target property.
    /// </summary>
    public async Task ReadSettings() {
        var ds = GetDeviceSettingsInfo();
        foreach (var info in ds) await InvokeSettingsMethod(info, nameof(IDeviceSetting<object>.Read));
    }

    public async Task WriteSettings() {
        var ds = GetDeviceSettingsInfo();
        foreach (var info in ds) await InvokeSettingsMethod(info, nameof(IDeviceSetting<object>.CommitChanges));
    }

    private async Task InvokeSettingsMethod(PropertyInfo info, string methodName) {
        var method = info.PropertyType.GetMethod(methodName);
        var setting = info.GetValue(this);
        if (setting == null || method == null) return;

        // dynamic awaitable = m?.Invoke(setting, null) ?? throw new InvalidOperationException();
        // if (awaitable != null) await awaitable;
        var task = (Task)method.Invoke(setting, null);
        if (task != null) await task;
    }

    private IEnumerable<PropertyInfo> GetDeviceSettingsInfo() {
        var propertyInfos = GetType().GetProperties();
        return propertyInfos.Where(info => info.PropertyType.IsGenericType)
            .Where(info => info.PropertyType.GetGenericTypeDefinition() == typeof(IDeviceSetting<>)).ToList();
    }
}