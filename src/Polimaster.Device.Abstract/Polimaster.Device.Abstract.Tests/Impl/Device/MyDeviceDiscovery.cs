using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Tests.Impl.Device.Transport;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device; 

public class MyDeviceDiscovery : ATransportDiscovery {
    protected override int Sleep => 1;
    
    private bool _inProgress;

    protected override void Search() {
        if (_inProgress) {
            Logger?.LogDebug("Previous search devices operation is not completed, skip");
            return;
        }

        _inProgress = true;

        // simulate found
        IEnumerable<ITransport> found = new List<ITransport> {
            new MyTransport(new MyClient(new MemoryStreamParams(), LoggerFactory), LoggerFactory)
        };
        Found?.Invoke(found);
        
        // simulate lost
        IEnumerable<ITransport> lost = new List<ITransport> {
            new MyTransport(new MyClient(new MemoryStreamParams(), LoggerFactory), LoggerFactory)
        };
        Lost?.Invoke(lost);
        
        _inProgress = false;
    }

    public MyDeviceDiscovery(ILoggerFactory? loggerFactory) : base(loggerFactory) {
    }
}