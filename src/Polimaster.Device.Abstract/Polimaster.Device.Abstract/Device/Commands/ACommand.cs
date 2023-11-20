using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <inheritdoc cref="Polimaster.Device.Abstract.Device.Commands.ICommand" />
public abstract class ACommand<T> : CommandBase, ICommand {
    /// <summary>
    /// Compile command
    /// </summary>
    /// <returns>Compiled command to send to <see cref="IDeviceStream{T}"/></returns>
    protected abstract T Compile();

    /// <inheritdoc />
    public virtual async Task Send<TStream>(TStream stream, CancellationToken cancellationToken) {
        if (stream is not IDeviceStream<T> str) throw new Exception($"{nameof(T)} is not suitable for writing to {nameof(TStream)}");
        LogCommand(nameof(Send));
        await str.WriteAsync(Compile(), cancellationToken);
    }

    /// <inheritdoc />
    protected ACommand(ILoggerFactory? loggerFactory) : base(loggerFactory) {
    }
}