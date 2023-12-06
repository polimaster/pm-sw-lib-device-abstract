using System.Threading;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Transport.Win.Usb.Tests; 

public class Mocks {
    protected static readonly ILoggerFactory? LOGGER_FACTORY = LoggerFactory.Create(builder => builder.AddConsole());
    protected readonly CancellationToken Token = new();
}