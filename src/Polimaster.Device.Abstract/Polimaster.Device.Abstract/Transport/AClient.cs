using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Transport;

/// <inheritdoc />
public abstract class AClient<TConnectionParams> : IClient<TConnectionParams> {
    /// <inheritdoc />
    public abstract void Dispose();

    /// <inheritdoc />
    public abstract bool Connected { get; }

    /// <inheritdoc />
    public ILoggerFactory? LoggerFactory { get; set; }

    /// <inheritdoc />
    public abstract void Close();

    /// <inheritdoc />
    public abstract Task<IDeviceStream> GetStream();

    /// <inheritdoc />
    public abstract void Open(TConnectionParams connectionParams);

    /// <inheritdoc />
    public abstract Task OpenAsync(TConnectionParams connectionParams);

    /// <inheritdoc />
    public abstract Action? Opened { get; set; }

    /// <inheritdoc />
    public abstract Action? Closed { get; set; }
}