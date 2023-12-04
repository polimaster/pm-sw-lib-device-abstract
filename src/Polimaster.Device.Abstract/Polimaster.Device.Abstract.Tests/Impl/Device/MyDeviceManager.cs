using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device; 

public class MyDeviceManager : ADeviceManager<IMyDeviceDiscovery, MyDevice> {

    public MyDeviceManager(IMyDeviceDiscovery discovery, ILoggerFactory? loggerFactory) : base(discovery, loggerFactory) {
    }
    protected override MyDevice FromTransport(ITransport transport) => new(transport, LoggerFactory);
}