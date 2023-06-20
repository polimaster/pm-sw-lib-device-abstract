using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Commands;
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
    public virtual async Task Write<TParam>(ICommand<TParam, TData> command, CancellationToken cancellationToken = new()) {
        try {
            command.Validate();
            Logger?.LogDebug("Executing command {C}", nameof(command.GetType));
            var stream = await Transport.Open();
            if (stream == null) throw new NullReferenceException("Transport stream is null");
            await Transport.Write(stream, command.Compile(), cancellationToken);
        } catch (CommandValidationException) {
            throw;
        } catch (Exception e) { throw new DeviceException(e); }
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
        } catch (CommandValidationException) {
            throw;
        } catch (CommandResultParsingException) {
            throw;
        } catch (Exception e) { throw new DeviceException(e); }
    }

    public void Dispose() {
        Transport.Dispose();
    }
}