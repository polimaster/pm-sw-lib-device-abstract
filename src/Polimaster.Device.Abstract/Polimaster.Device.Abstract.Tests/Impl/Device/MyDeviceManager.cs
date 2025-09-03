using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Tests.Impl.Settings;
using Polimaster.Device.Abstract.Tests.Impl.Transport;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device;

public class MyDeviceManager(IMyDeviceDiscovery discovery, ILoggerFactory? loggerFactory)
    : ADeviceManager<IMyDevice, IMyTransport, IMyDeviceStream, IMyDeviceDiscovery, ClientParams>(discovery,
        loggerFactory) {
    private readonly IMySettingDescriptors _settingsDescriptors = new MySettingDescriptors();

    public override ISettingDescriptors SettingsDescriptors => _settingsDescriptors;

    protected override IMyDevice CreateDevice(IMyTransport transport) =>
        new MyDevice(transport, _settingsDescriptors, LoggerFactory);

    protected override IMyTransport CreateTransport(IClient<IMyDeviceStream> client) =>
        new MyTransport(client, LoggerFactory);

    protected override IClient<IMyDeviceStream> CreateClient(ClientParams connectionParams) =>
        new MyClient(connectionParams, LoggerFactory);
}