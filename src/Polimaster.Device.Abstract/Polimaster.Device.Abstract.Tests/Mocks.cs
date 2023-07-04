using System.IO;
using Microsoft.Extensions.Logging;
using Moq;
using Polimaster.Device.Abstract.Device.Commands.Interfaces;
using Polimaster.Device.Abstract.Device.Interfaces;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;
using Polimaster.Device.Abstract.Transport.Interfaces;

namespace Polimaster.Device.Abstract.Tests; 

public class Mocks {
    public static readonly ILoggerFactory? LOGGER_FACTORY = LoggerFactory.Create(builder => builder.AddConsole());
    
    public static Mock<ITransport<string>> TransportMock => new ();
    public static Mock<IDevice<string>> DeviceMock => new ();
    
    public static Mock<Stream> StreamMock => new();
    public static Mock<ICommand<string, string>> CommandMock => new();
    public static Mock<ICommandBuilder<string>> CommandBuilderMock => new();
    
    public static Mock<IDeviceSetting<string>> SettingMock => new();
    public static Mock<IDeviceSettingBuilder<string>> SettingBuilderMock => new();
}