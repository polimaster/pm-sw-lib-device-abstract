using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <summary>
/// Write command
/// </summary>
/// <typeparam name="TValue">Type of command result <see cref="ICommand"/></typeparam>
/// <typeparam name="TCommand">Command type</typeparam>
public abstract class WriteCommand<TValue, TCommand> : ACommand<TValue, TCommand> {
    /// <summary>
    /// Command constructor
    /// </summary>
    /// <param name="loggerFactory">Logger factory</param>
    protected WriteCommand(ILoggerFactory? loggerFactory = null) : base(loggerFactory) {
    }

    /// <inheritdoc />
    public override async Task Send(IDeviceStream stream, ushort sleep, CancellationToken cancellationToken = new()) {
        await Write(stream, sleep, cancellationToken);
        ValueChanged?.Invoke(Value);
    }

    /// <summary>
    /// Write command
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="sleep"></param>
    /// <param name="cancellationToken"></param>
    protected async Task Write(IDeviceStream stream, ushort sleep, CancellationToken cancellationToken) {
        try {
            LogCommand(nameof(Write));
            Validate();
            var command = Compile();
            await WriteData(stream, command, cancellationToken);
            Thread.Sleep(sleep);
        } catch (Exception e) {
            LogError(e, nameof(Write));
            throw;
        }
    }

    /// <summary>
    /// Write data to stream
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected abstract Task WriteData(IDeviceStream stream, TCommand command, CancellationToken cancellationToken);
}