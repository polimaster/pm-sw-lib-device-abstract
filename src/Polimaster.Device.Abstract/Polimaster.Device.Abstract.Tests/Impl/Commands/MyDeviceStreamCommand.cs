using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Tests.Impl.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Commands;

public abstract class MyDeviceStreamCommand(IMyTransport transport, ILoggerFactory? loggerFactory)
    : ACommand<IMyDeviceStream>(transport, loggerFactory) {
    
    protected abstract byte[] Compile();

    protected override async Task Execute(IMyDeviceStream stream, CancellationToken token) {
        await stream.Write(Compile(), token);
    }
}