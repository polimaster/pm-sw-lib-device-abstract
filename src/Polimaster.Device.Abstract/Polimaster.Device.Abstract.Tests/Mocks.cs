using System.Threading;
using Microsoft.Extensions.Logging;
using Moq;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests; 

public class Mocks {
    protected static readonly ILoggerFactory? LOGGER_FACTORY = LoggerFactory.Create(builder => builder.AddConsole());
    protected static Mock<ITransport> TransportMock => new ();

    protected readonly CancellationToken Token = new();
    // protected static Mock<IDevice> DeviceMock => new ();
    //
    // protected static Mock<Stream> StreamMock => new();
    // protected static Mock<IDeviceStream> DeviceStreamMock => new();
    // protected static Mock<ICommand<MyParam>> CommandMock => new();
    //
    // protected static Mock<IDeviceSetting<string>> SettingMock => new();
    // protected static Mock<ISettingBuilder> SettingBuilderMock => new();
}