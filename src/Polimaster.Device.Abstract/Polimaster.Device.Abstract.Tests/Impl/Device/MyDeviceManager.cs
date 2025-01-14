using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Tests.Impl.Device.Transport;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device; 

public class MyDeviceManager(IMyDeviceDiscovery discovery, ILoggerFactory? loggerFactory)
    : ADeviceManager<IMyDevice, IMyDeviceDiscovery, ClientParams>(discovery, loggerFactory) {
    protected override IMyDevice CreateDevice(ITransport transport) => new MyDevice(transport, LoggerFactory);

    protected override ITransport CreateTransport(IClient client) => new MyTransport(client, LoggerFactory);

    protected override IClient CreateClient(ClientParams connectionParams) => new MyClient(connectionParams, LoggerFactory);
}