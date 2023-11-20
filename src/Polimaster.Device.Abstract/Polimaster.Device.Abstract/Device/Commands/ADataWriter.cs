using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Commands;


/// <summary>
/// Device data writer
/// </summary>
/// <typeparam name="T">Type of data to write</typeparam>
/// <typeparam name="TSteamData">Data type for device <see cref="IDeviceStream{T}"/></typeparam>
public abstract class ADataWriter<T, TSteamData> : CommandBase, IDataWriter<T> {
    /// <inheritdoc />
    protected ADataWriter(ILoggerFactory? loggerFactory) : base(loggerFactory) {
    }

    /// <summary>
    /// Compile command
    /// </summary>
    /// <param name="data">Data to write</param>
    /// <returns></returns>
    protected abstract TSteamData Compile(T? data);

    /// <inheritdoc />
    public virtual async Task Write<TStream>(TStream stream, T? data, CancellationToken cancellationToken) {
        if (stream is not IDeviceStream<TSteamData> str) throw new Exception($"{nameof(TSteamData)} is not suitable for writing to {nameof(TStream)}");
        LogCommand(nameof(Write));
        await str.WriteAsync(Compile(data), cancellationToken);
    }
}