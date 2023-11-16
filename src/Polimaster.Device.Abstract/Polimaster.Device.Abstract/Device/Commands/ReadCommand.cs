using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Exceptions;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Commands;


/// <summary>
/// Read command
/// </summary>
/// <inheritdoc />
public abstract class ReadCommand<TValue, TCommand> : WriteCommand<TValue, TCommand> {

    /// <inheritdoc />
    protected ReadCommand(ILoggerFactory? loggerFactory = null) : base(loggerFactory) {
    }
    
    /// <summary>
    /// Parse data returned by <see cref="IDeviceStream"/> while read
    /// </summary>
    /// <param name="result">Value for parsing</param>
    /// <exception cref="CommandResultParsingException"></exception>
    /// <returns></returns>
    protected abstract TValue? Parse(TCommand? result);

    /// <summary>
    /// Check command result value. Should throw exception if fails.
    /// </summary>
    /// <param name="value"></param>
    protected virtual void CheckResult(TValue? value) {
        if (value == null) throw new NullReferenceException("Command result is null");
    }

    /// <inheritdoc />
    public override async Task Send(IDeviceStream stream, ushort sleep, CancellationToken cancellationToken = new()) {
        await Write(stream, sleep, cancellationToken);
        await Read(stream, sleep, cancellationToken);
        ValueChanged?.Invoke(Value);
    }

    private async Task Read(IDeviceStream stream, ushort sleep, CancellationToken cancellationToken) {
        try {
            LogCommand(nameof(Read));
            var data = await ReadData(stream, cancellationToken);
            var res = Parse(data);
            CheckResult(res);
            Value = res;
            Thread.Sleep(sleep);
        } catch (Exception e) {
            LogError(e, nameof(Read));
            throw;
        }
    }

    /// <summary>
    /// Read data from stream
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected abstract Task<TCommand> ReadData(IDeviceStream stream, CancellationToken cancellationToken);
}