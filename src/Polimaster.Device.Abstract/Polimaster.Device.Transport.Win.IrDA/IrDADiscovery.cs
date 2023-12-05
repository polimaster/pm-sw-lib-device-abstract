using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Transport.Win.IrDA;

/// <inheritdoc />
public abstract class IrDADiscovery : ATransportDiscovery {
    private bool _inDiscoveringState;
    
    /// <summary>
    /// IrDA device identifier (Service name)
    /// </summary>
    protected abstract string DeviceIdentifier { get; }

    /// <inheritdoc />
    protected IrDADiscovery(ILoggerFactory? loggerFactory) : base(loggerFactory) {
    }

    /// <inheritdoc />
    protected override void Search() {
        if (_inDiscoveringState) {
            Logger?.LogDebug("Previous search devices operation is not completed, skip");
            return;
        }

        try {
            _inDiscoveringState = true;
            
            var di = IrDaClientAdapter.DiscoverDevices(DeviceIdentifier);
            var res = di.Select(device => new IrDATransport(new IrDaClientAdapter(device, LoggerFactory), LoggerFactory));
            
            Found?.Invoke(res.ToList());
        } finally { _inDiscoveringState = false; }
    }
    

    /// <inheritdoc />
    public override event Action<IEnumerable<ITransport>>? Found;

    /// <inheritdoc />
    public override event Action<IEnumerable<ITransport>>? Lost;
}