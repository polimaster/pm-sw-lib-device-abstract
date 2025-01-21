using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Transport;

public interface IMyTransport : ITransport<IMyDeviceStream>;

public class MyTransport(IClient<IMyDeviceStream> client, ILoggerFactory? loggerFactory)
    : ATransport<IMyDeviceStream>(client, loggerFactory), IMyTransport;