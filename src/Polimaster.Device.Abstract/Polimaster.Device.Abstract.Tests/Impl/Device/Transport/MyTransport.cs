using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Transport; 

public class MyTransport(IClient client, ILoggerFactory? loggerFactory)
    : ATransport(client, loggerFactory);