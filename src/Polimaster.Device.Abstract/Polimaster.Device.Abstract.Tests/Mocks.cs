using System.IO;
using Microsoft.Extensions.Logging;
using Moq;
using Polimaster.Device.Abstract.Device.Commands.Interfaces;
using Polimaster.Device.Abstract.Device.Interfaces;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;
using Polimaster.Device.Abstract.Transport.Interfaces;

namespace Polimaster.Device.Abstract.Tests; 

public class Mocks {
    protected static readonly ILoggerFactory? LOGGER_FACTORY = LoggerFactory.Create(builder => builder.AddConsole());

    protected static Mock<ITransport<string>> TransportMock => new ();
    protected static Mock<IDevice<string>> DeviceMock => new ();

    protected static Mock<Stream> StreamMock => new();
    protected static Mock<ICommand<string, string>> CommandMock => new();

    protected static Mock<IDeviceSetting<string>> SettingMock => new();
    protected static Mock<IDeviceSettingBuilder> SettingBuilderMock => new();
}