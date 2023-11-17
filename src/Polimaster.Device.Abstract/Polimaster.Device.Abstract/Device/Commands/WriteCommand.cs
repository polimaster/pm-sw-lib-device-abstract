using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <summary>
/// Write command
/// </summary>
/// <typeparam name="TValue">Type of command result <see cref="ICommand{TValue,TStream}"/></typeparam>
/// <typeparam name="TCommand">Command type</typeparam>
public abstract class WriteCommand<TValue, TCommand> : ACommand<TValue, TCommand> {
    /// <summary>
    /// Command constructor
    /// </summary>
    /// <param name="loggerFactory">Logger factory</param>
    protected WriteCommand(ILoggerFactory? loggerFactory = null) : base(loggerFactory) {
    }

    /// <inheritdoc />
    public override async Task<TValue?> Send<TStream>(TStream stream,
        TValue? value,
        CancellationToken cancellationToken = new()) {
        await Write(value, stream, cancellationToken);
        return default;
    }

    /// <summary>
    /// Write command
    /// </summary>
    /// <param name="value"></param>
    /// <param name="stream"></param>
    /// <param name="cancellationToken"></param>
    protected async Task Write<TStream>(TValue? value, TStream stream, CancellationToken cancellationToken) where TStream : IDeviceStream<TCommand> {
        try {
            LogCommand(nameof(Write));
            Validate();
            var command = Compile(value);
            await WriteData(stream, command, cancellationToken);
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
    protected abstract Task WriteData<TStream>(TStream stream, TCommand command, CancellationToken cancellationToken) where TStream : IDeviceStream<TCommand>;
}