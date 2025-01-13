using System.Threading;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Tests; 

public class Mocks {
    protected static readonly ILoggerFactory? LOGGER_FACTORY = LoggerFactory.Create(builder => builder.AddConsole());
    protected readonly CancellationToken Token = CancellationToken.None;
}