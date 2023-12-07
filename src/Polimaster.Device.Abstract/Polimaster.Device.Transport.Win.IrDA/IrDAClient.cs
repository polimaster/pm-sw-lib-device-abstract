using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;
using Polimaster.Device.Abstract.Transport.Stream;
using Polimaster.Device.Abstract.Transport.Stream.Socket;

namespace Polimaster.Device.Transport.Win.IrDA;

/// <inheritdoc cref="Polimaster.Device.Abstract.Transport.AClient{T,TConnectionParams}" />
public class IrDAClient : AClient<byte[], IrDaDevice> {
    
    private InTheHand.Net.Sockets.IrDAClient? _wrapped;
    
    /// <inheritdoc />
    public override bool Connected =>  _wrapped is { Connected: true };
    
    /// <inheritdoc />
    public IrDAClient(IrDaDevice @params, ILoggerFactory? loggerFactory) : base(@params, loggerFactory) {
    }

    /// <inheritdoc />
    public override void Close() {
        _wrapped?.Close();
        _wrapped?.Dispose();
        _wrapped = null;
    }

    /// <inheritdoc />
    public override void Reset() {
        Close();
        _wrapped = new InTheHand.Net.Sockets.IrDAClient();
    }

    /// <inheritdoc />
    public override IDeviceStream<byte[]> GetStream() {
        if (_wrapped is not { Connected: true }) throw new DeviceClientException($"{_wrapped?.GetType().Name} is closed or null");
        return new IrDAStream(new SocketWrapper(_wrapped.Client, true), LoggerFactory);
    }

    /// <inheritdoc />
    public override void Open() {
        if (_wrapped is { Connected: true }) return;
        Reset();
        _wrapped?.Connect(Params.Name);
    }

    /// <inheritdoc />
    public override Task OpenAsync(CancellationToken token) {
        if (_wrapped is { Connected: true }) return Task.CompletedTask;
        Reset();
        _wrapped?.BeginConnect(Params.Name, null, _wrapped).AsyncWaitHandle.WaitOne(1000);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override void Dispose() => Close();

    
    /// <summary>
    /// Discover connected devices
    /// </summary>
    /// <param name="deviceIdentifier">IrDa service name</param>
    /// <returns></returns>
    public static IEnumerable<IrDaDevice> DiscoverDevices(string deviceIdentifier) {
        var c = new InTheHand.Net.Sockets.IrDAClient();
        var d = c.DiscoverDevices(1);
        return from info in d
            where info.DeviceName.Contains(deviceIdentifier)
            select new IrDaDevice { Name = info.DeviceName };
    }
}