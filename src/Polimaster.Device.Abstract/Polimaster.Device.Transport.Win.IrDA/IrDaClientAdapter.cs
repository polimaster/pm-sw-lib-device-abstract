using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InTheHand.Net.Sockets;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Transport.Win.IrDA;

/// <inheritdoc cref="Polimaster.Device.Abstract.Transport.AClient{T,TConnectionParams}" />
public class IrDaClientAdapter : AClient<byte[], IrDaDevice> {
    
    private IrDAClient _wrapped;
    
    
    /// <inheritdoc />
    public override bool Connected => _wrapped.Connected;
    
    /// <inheritdoc />
    public IrDaClientAdapter(IrDaDevice @params, ILoggerFactory? loggerFactory) : base(@params, loggerFactory) {
        _wrapped = new IrDAClient();
    }

    /// <inheritdoc />
    public override void Close() {
        _wrapped.Close();
    }

    /// <inheritdoc />
    public override IDeviceStream<byte[]> GetStream() => 
        new IrDAStream(_wrapped.GetStream(), LoggerFactory);

    /// <inheritdoc />
    public override void Open() {
        _wrapped.Connect(Params.Name);
    }

    /// <inheritdoc />
    public override Task OpenAsync(CancellationToken token) {
        if(_wrapped.Connected) _wrapped.BeginConnect(Params.Name, null, _wrapped).AsyncWaitHandle.WaitOne(1000);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override void Dispose() {
        Close();
        _wrapped.Dispose();
    }

    /// <inheritdoc />
    public static IEnumerable<IrDaDevice> DiscoverDevices(string deviceIdentifier) {
        var c = new IrDAClient();
        var d = c.DiscoverDevices(1);
        return (from info in d
            where info.DeviceName.Contains(deviceIdentifier)
            select new IrDaDevice { Name = info.DeviceName }).ToList();
    }
}