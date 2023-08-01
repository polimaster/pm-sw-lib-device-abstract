using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Transport;

/// <inheritdoc />
public abstract class ATransport<TConnectionParams> : ITransport<TConnectionParams> {
    /// <inheritdoc />
    public string ConnectionId => $"{GetType().Name}:{ConnectionParams}";

    /// <inheritdoc />
    public IClient<TConnectionParams> Client { get; protected set; }

    /// <inheritdoc />
    public TConnectionParams ConnectionParams { get; protected set; }

    /// <inheritdoc />
    public Action? Opened { get; set; }

    /// <inheritdoc />
    public Action? Closed { get; set; }
    
    // protected readonly ILogger<ITransport<TConnectionParams>>? Logger;


    /// <summary>
    /// </summary>
    /// <param name="client"></param>
    /// <param name="connectionParams">Parameters for underlying client, <see cref="ConnectionParams" /></param>
    /// <param name="loggerFactory"></param>
    protected ATransport(IClient<TConnectionParams> client, TConnectionParams connectionParams,
        ILoggerFactory? loggerFactory = null) {
        ConnectionParams = connectionParams;
        Client = client;
        Client.LoggerFactory = loggerFactory;

        Client.Opened += () => Opened?.Invoke();
        Client.Closed += () => Closed?.Invoke();

        // Logger = loggerFactory?.CreateLogger<ITransport<TConnectionParams>>();
    }

    /// <inheritdoc />
    public virtual Task<IDeviceStream> Open() {
        Client.Open(ConnectionParams);
        return Client.GetStream();
    }

    /// <inheritdoc />
    public virtual Task Close() {
        Client.Close();
        return Task.CompletedTask;
    }


    /// <inheritdoc />
    public virtual void Dispose() {
        Close();
        Client.Dispose();
    }
}