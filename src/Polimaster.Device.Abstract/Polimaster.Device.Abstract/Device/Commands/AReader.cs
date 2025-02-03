using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Exceptions;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <summary>
/// Device data reader
/// </summary>
/// <typeparam name="TValue">Type of value to read</typeparam>
/// <typeparam name="TData">Type of data to read/write from <typeparamref name="TStream"/></typeparam>
/// <typeparam name="TStream">See <see cref="ITransport{TStream}"/></typeparam>
public abstract class AReader<TValue, TData, TStream> : CommandBase<TStream>, IDataReader<TValue>
    where TData : notnull
    where TValue : notnull {

    /// <summary>
    /// Parse data received from device
    /// </summary>
    /// <param name="res"></param>
    /// <returns></returns>
    /// <exception cref="CommandResultParsingException"></exception>
    protected abstract TValue Parse(TData? res);

    /// <inheritdoc />
    public virtual async Task<TValue> Read(CancellationToken cancellationToken) {
        LogDebug(nameof(Read));
        try {
            // await Transport.WriteAsync(Compile(), cancellationToken);
            // var res = await Transport.ReadAsync(cancellationToken);
            var res = default(TData);
            await Transport.ExecOnStream(async stream => {
                res = await Execute(stream, cancellationToken);
            }, cancellationToken);
            return Parse(res);
        } catch (Exception e) {
            LogError(e, nameof(Read));
            throw;
        }
    }

    /// <summary>
    /// Execute command on stream and return result
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected abstract Task<TData> Execute(TStream stream, CancellationToken cancellationToken);

    /// <inheritdoc />
    protected AReader(ITransport<TStream> transport, ILoggerFactory? loggerFactory) : base(transport, loggerFactory) {
    }
}