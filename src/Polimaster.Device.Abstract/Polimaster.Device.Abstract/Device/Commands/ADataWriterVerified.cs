using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;
using Polimaster.Device.Abstract.Transport.Stream;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <summary>
/// Device data writer with verifying result returned from device
/// </summary>
/// <typeparam name="T">Type of data to write</typeparam>
/// <typeparam name="TSteamData">Data type for device <see cref="IDeviceStream{T}"/></typeparam>
public abstract class ADataWriterVerified<T, TSteamData>(ITransport<TSteamData> transport, ILoggerFactory? loggerFactory)
    : ADataWriter<T, TSteamData>(transport, loggerFactory) {
    /// <inheritdoc />
    public override async Task Write(T data, CancellationToken cancellationToken) {
        await base.Write(data, cancellationToken);
        var response = await Transport.ReadAsync(cancellationToken);
        try {
            Verify(response);
        } catch (Exception e) {
            throw new CommandResultException(e);
        }
    }

    /// <summary>
    /// Verify data returned from device. Should throw exception if response data incorrect.
    /// </summary>
    /// <param name="response"></param>
    protected abstract void Verify(TSteamData? response);
}