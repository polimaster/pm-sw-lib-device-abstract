using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Tests.Impl.Device.Transport;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device; 

public class MyDeviceManager(IMyDeviceDiscovery discovery, ILoggerFactory? loggerFactory)
    : ADeviceManager<MyDevice, string, IMyDeviceDiscovery, ClientParams>(discovery, loggerFactory) {
    protected override MyDevice CreateDevice(ITransport<string> transport) => new(transport, LoggerFactory);

    protected override ITransport<string> CreateTransport(IClient<string> client) => new MyTransport(client, LoggerFactory);

    protected override IClient<string> CreateClient(ClientParams connectionParams) => new MyClient(connectionParams, LoggerFactory);
}