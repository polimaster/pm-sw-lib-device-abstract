using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Exceptions;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Commands;


/// <summary>
/// Device data writer
/// </summary>
/// <typeparam name="TValue">Type of data to write</typeparam>
/// <typeparam name="TStream">See <see cref="ITransport{TStream}"/></typeparam>
public abstract class AWriter<TValue, TStream>(ITransport<TStream> transport, ILoggerFactory? loggerFactory)
    : CommandBase<TStream>(transport, loggerFactory), IDataWriter<TValue> where TValue : notnull {
    /// <inheritdoc />
    public abstract Task Write(TValue data, CancellationToken cancellationToken);
}

/// <summary>
/// Device data writer
/// </summary>
/// <typeparam name="TValue">Type of data to write</typeparam>
/// <typeparam name="TData">Type of data to read/write from <typeparamref name="TStream"/></typeparam>
/// <typeparam name="TStream">See <see cref="ITransport{TStream}"/></typeparam>
public abstract class AWriter<TValue, TData, TStream> : AWriter<TValue, TStream>
    where TData : notnull
    where TValue : notnull {
    /// <summary>
    /// Compile <paramref name="value"/> to <typeparamref name="TData"/> type before send to <typeparamref name="TStream"/>
    /// </summary>
    /// <param name="value">Data to write</param>
    /// <returns>Compiled value</returns>
    /// <exception cref="CommandCompilationException"></exception>
    protected abstract TData Compile(TValue value);

    /// <inheritdoc />
    public override async Task Write(TValue data, CancellationToken cancellationToken) {
        LogDebug(nameof(Write));
        try {
            // await Transport.WriteAsync(Compile(data), cancellationToken);
            await Transport.ExecOnStream(stream => Execute(stream, Compile(data), cancellationToken), cancellationToken);
        } catch (Exception e) {
            LogError(e, nameof(Write));
            throw;
        }
    }

    /// <summary>
    /// Execute command on stream
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="compiled"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected abstract Task Execute(TStream stream, TData compiled, CancellationToken cancellationToken);

    /// <inheritdoc />
    protected AWriter(ITransport<TStream> transport, ILoggerFactory? loggerFactory) : base(transport, loggerFactory) {
    }
}