using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Tests.Impl.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Commands;

public abstract class MyDeviceStreamReader<T>(IMyTransport transport, ILoggerFactory? loggerFactory)
    : ADataReader<T, byte[], IMyDeviceStream>(transport, loggerFactory) {

    protected abstract byte[] Compile();

    protected override async Task<byte[]> Execute(IMyDeviceStream stream, CancellationToken cancellationToken) {
        await stream.Write(Compile(), cancellationToken);
        return await stream.Read(cancellationToken);
    }
}