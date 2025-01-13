using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Exceptions;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <inheritdoc cref="Polimaster.Device.Abstract.Device.Commands.ICommand" />
public abstract class ACommand<T> : CommandBase<T>, ICommand {
    /// <summary>
    /// Compile command
    /// </summary>
    /// <returns>Compiled command to send to <see cref="Transport"/></returns>
    /// <exception cref="CommandCompilationException"></exception>
    protected abstract T Compile();

    /// <inheritdoc />
    public virtual async Task Exec(CancellationToken cancellationToken) {
        LogCommand(nameof(Exec));
        try {
            await Transport.WriteAsync(Compile(), cancellationToken);
        } catch (Exception e) {
            LogError(e, nameof(Exec));
            throw;
        }
    }

    /// <inheritdoc />
    protected ACommand(ITransport<T> transport, ILoggerFactory? loggerFactory) : base(transport, loggerFactory) {
    }
}