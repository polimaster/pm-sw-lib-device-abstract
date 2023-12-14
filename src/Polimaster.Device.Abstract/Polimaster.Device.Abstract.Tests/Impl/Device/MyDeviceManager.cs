using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device; 

public class MyDeviceManager(IMyDeviceDiscovery discovery, ILoggerFactory? loggerFactory)
    : ADeviceManager<IMyDeviceDiscovery, MyDevice>(discovery, loggerFactory) {
    protected override MyDevice FromTransport(ITransport transport) => new(transport, LoggerFactory);
}