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
    /// <summary>
    /// Underlying wrapped IrDA client
    /// </summary>
    protected InTheHand.Net.Sockets.IrDAClient? Wrapped;
    
    /// <inheritdoc />
    public override bool Connected =>  Wrapped is { Connected: true };
    
    /// <inheritdoc />
    public IrDAClient(IrDaDevice @params, ILoggerFactory? loggerFactory) : base(@params, loggerFactory) {
    }

    /// <inheritdoc />
    public override void Close() {
        try {
            Wrapped?.Close();
            Wrapped?.Dispose();
        } finally {
            Wrapped = null;
        }
    }

    /// <inheritdoc />
    public override void Reset() {
        Close();
        Wrapped = new InTheHand.Net.Sockets.IrDAClient();
    }

    /// <inheritdoc />
    public override IDeviceStream<byte[]> GetStream() {
        if (Wrapped is not { Connected: true }) throw new DeviceClientException($"{Wrapped?.GetType().Name} is closed or null");
        return new IrDAStream(new SocketWrapper(Wrapped.Client, true), LoggerFactory);
    }

    /// <inheritdoc />
    public override void Open() {
        if (Wrapped is { Connected: true }) return;
        Reset();
        Wrapped?.Connect(Params.Name);
    }

    /// <inheritdoc />
    public override Task OpenAsync(CancellationToken token) {
        Open();
        // if (_wrapped is { Connected: true }) return Task.CompletedTask;
        // Reset();
        // _wrapped?.BeginConnect(Params.Name, null, _wrapped).AsyncWaitHandle.WaitOne(1000);
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