using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Tests.Impl.Transport;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device; 

public class MyDeviceManager(IMyDeviceDiscovery discovery, ILoggerFactory? loggerFactory)
    : ADeviceManager<IMyDevice, IMyTransport, IMyDeviceStream , IMyDeviceDiscovery, ClientParams>(discovery, loggerFactory) {
    protected override IMyDevice CreateDevice(IMyTransport transport) => new MyDevice(transport, LoggerFactory);

    protected override IMyTransport CreateTransport(IClient<IMyDeviceStream> client) => new MyTransport(client, LoggerFactory);

    protected override IClient<IMyDeviceStream> CreateClient(ClientParams connectionParams) => new MyClient(connectionParams, LoggerFactory);
}