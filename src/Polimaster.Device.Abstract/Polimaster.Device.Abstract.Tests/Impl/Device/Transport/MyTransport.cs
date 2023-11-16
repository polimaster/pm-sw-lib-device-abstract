using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Transport; 

public class MyTransport : ATransport<MyClient, ConnectionParams> {
    public MyTransport(ConnectionParams connectionParams, ILoggerFactory? loggerFactory = null) : base(connectionParams, loggerFactory) {
    }
}