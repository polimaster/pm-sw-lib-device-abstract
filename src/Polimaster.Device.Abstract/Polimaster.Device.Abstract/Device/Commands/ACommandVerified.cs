using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <summary>
/// Command with verifying result returned from device
/// </summary>
/// <param name="loggerFactory"></param>
/// <typeparam name="T"></typeparam>
public abstract class ACommandVerified<T>(ITransport<T> transport, ILoggerFactory? loggerFactory) : ACommand<T>(transport, loggerFactory) {
    /// <inheritdoc />
    public override async Task Exec(CancellationToken cancellationToken) {
        await base.Exec(cancellationToken);
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
    protected abstract void Verify(T? response);
}