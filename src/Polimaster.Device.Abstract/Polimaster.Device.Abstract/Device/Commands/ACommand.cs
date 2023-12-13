using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Exceptions;
using Polimaster.Device.Abstract.Transport.Stream;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <inheritdoc cref="Polimaster.Device.Abstract.Device.Commands.ICommand" />
public abstract class ACommand<T> : CommandBase<T>, ICommand {
    /// <summary>
    /// Compile command
    /// </summary>
    /// <returns>Compiled command to send to <see cref="IDeviceStream{T}"/></returns>
    /// <exception cref="CommandCompilationException"></exception>
    protected abstract T Compile();

    /// <inheritdoc />
    public virtual async Task Exec<TStream>(TStream stream, CancellationToken cancellationToken) {
        var str = GetStream(stream);
        LogCommand(nameof(Exec));
        try {
            await str.WriteAsync(Compile(), cancellationToken);
        } catch (Exception e) {
            LogError(e, nameof(Exec));
            throw;
        }
    }

    /// <inheritdoc />
    protected ACommand(ILoggerFactory? loggerFactory) : base(loggerFactory) {
    }
}