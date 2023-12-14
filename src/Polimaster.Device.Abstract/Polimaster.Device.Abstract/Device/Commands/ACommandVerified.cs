using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <summary>
/// Command with verifying result returned from device
/// </summary>
/// <param name="loggerFactory"></param>
/// <typeparam name="T"></typeparam>
public abstract class ACommandVerified<T>(ILoggerFactory? loggerFactory) : ACommand<T>(loggerFactory) {
    /// <inheritdoc />
    public override async Task Exec<TStream>(TStream stream, CancellationToken cancellationToken) {
        await base.Exec(stream, cancellationToken);
        var str = GetStream(stream);
        var response = await str.ReadAsync(cancellationToken);
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