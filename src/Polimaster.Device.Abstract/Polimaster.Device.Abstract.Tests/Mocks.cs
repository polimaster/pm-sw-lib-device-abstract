using System.IO;
using Microsoft.Extensions.Logging;
using Moq;
using Polimaster.Device.Abstract.Transport.Interfaces;

namespace Polimaster.Device.Abstract.Tests; 

public class Mocks {
    public readonly ILoggerFactory? LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder => builder.AddConsole());
    
    public static Mock<ITransport<string>> TransportMock => new ();
    public static Mock<Stream> StreamMock => new();
}