using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Tests.Impl.Device.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device;

public interface IMyDeviceDiscovery : ITransportDiscovery<ClientParams>;

public class MyDeviceDiscovery(ILoggerFactory? loggerFactory) : ATransportDiscovery<ClientParams>(loggerFactory), IMyDeviceDiscovery {
    protected override int Sleep => 1;
    
    private bool _inProgress;

    protected override void Search() {
        if (_inProgress) {
            Logger?.LogDebug("Previous search devices operation is not completed, skip");
            return;
        }

        _inProgress = true;

        // simulate found
        IEnumerable<ClientParams> found = new List<ClientParams> {
            new(1243, 80)
        };
        Found?.Invoke(found);
        
        // simulate lost
        IEnumerable<ClientParams> lost = new List<ClientParams> {
            new(4567, 80)
        };
        Lost?.Invoke(lost);
        
        _inProgress = false;
    }

    public override event Action<IEnumerable<ClientParams>>? Found;
    public override event Action<IEnumerable<ClientParams>>? Lost;
}