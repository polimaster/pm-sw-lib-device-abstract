using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Exceptions;
using Polimaster.Device.Abstract.Transport;
using Polimaster.Device.Abstract.Transport.Stream;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <summary>
/// Device data writer
/// </summary>
/// <typeparam name="T">Type of data to write</typeparam>
/// <typeparam name="TSteamData">Data type for device <see cref="IDeviceStream{T}"/></typeparam>
public abstract class ADataWriter<T, TSteamData> : CommandBase<TSteamData>, IDataWriter<T> {
    /// <summary>
    /// Compile command
    /// </summary>
    /// <param name="data">Data to write</param>
    /// <returns></returns>
    /// <exception cref="CommandCompilationException"></exception>
    protected abstract TSteamData Compile(T data);

    /// <inheritdoc />
    public virtual async Task Write(T data, CancellationToken cancellationToken) {
        LogCommand(nameof(Write));
        try {
            await Transport.WriteAsync(Compile(data), cancellationToken);
        } catch (Exception e) {
            LogError(e, nameof(Write));
            throw;
        }
    }


    /// <inheritdoc />
    protected ADataWriter(ITransport<TSteamData> transport, ILoggerFactory? loggerFactory) : base(transport, loggerFactory) {
    }
}