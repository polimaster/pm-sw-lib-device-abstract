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
/// <typeparam name="T">Type of data to write</typeparam>
public abstract class ADataReader<T> : CommandBase, IDataReader<T> {
    /// <inheritdoc />
    protected ADataReader(ITransport transport, ILoggerFactory? loggerFactory) : base(transport, loggerFactory) {
    }

    /// <summary>
    /// Compile command
    /// </summary>
    /// <returns></returns>
    /// <exception cref="CommandCompilationException"></exception>
    protected abstract byte[] Compile();

    /// <summary>
    /// Parse data received from device
    /// </summary>
    /// <param name="res"></param>
    /// <returns></returns>
    /// <exception cref="CommandResultParsingException"></exception>
    protected abstract T? Parse(byte[] res);

    /// <inheritdoc />
    public virtual async Task<T?> Read(CancellationToken cancellationToken) {
        LogCommand(nameof(Read));
        try {
            await Transport.WriteAsync(Compile(), cancellationToken);
            var res = await Transport.ReadAsync(cancellationToken);
            return Parse(res);
        } catch (Exception e) {
            LogError(e, nameof(Read));
            throw;
        }
    }
}