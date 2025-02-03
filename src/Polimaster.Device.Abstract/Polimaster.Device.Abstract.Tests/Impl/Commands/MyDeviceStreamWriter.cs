using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Tests.Impl.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Commands;

public abstract class MyDeviceStreamWriter<T>(IMyTransport transport, ILoggerFactory? loggerFactory)
    : AWriter<T, byte[], IMyDeviceStream>(transport, loggerFactory) {

    protected override async Task Execute(IMyDeviceStream stream, byte[] compiled, CancellationToken cancellationToken) {
        await stream.Write(compiled, cancellationToken);
    }
}