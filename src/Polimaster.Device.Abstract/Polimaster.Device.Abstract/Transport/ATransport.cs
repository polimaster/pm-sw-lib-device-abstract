using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Transport;

/// <inheritdoc />
public abstract class ATransport<TClient, TConnectionParams> : ITransport<TClient, TConnectionParams>
    where TClient : class, IClient<TConnectionParams>, new() {
    private readonly ILoggerFactory? _loggerFactory;

    /// <inheritdoc />
    public string ConnectionId => $"{GetType().Name}:{ConnectionParams}";

    /// <inheritdoc />
    public TClient? Client { get; protected set; }

    /// <inheritdoc />
    public TConnectionParams ConnectionParams { get; }

    /// <inheritdoc />
    public Action? Opened { get; set; }

    /// <inheritdoc />
    public Action? Closed { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="connectionParams"></param>
    /// <param name="loggerFactory"></param>
    protected ATransport(TConnectionParams connectionParams, ILoggerFactory? loggerFactory = null) {
        _loggerFactory = loggerFactory;
        ConnectionParams = connectionParams;
    }

    /// <inheritdoc />
    public virtual Task<IDeviceStream> Open() {
        if (Client == null) {
            Client = new TClient();
            Client.LoggerFactory = _loggerFactory;
            Client.Opened += () => Opened?.Invoke();
            Client.Closed += () => Closed?.Invoke();
        }

        Client.Open(ConnectionParams);
        return Client.GetStream();
    }

    /// <inheritdoc />
    public virtual Task Close() {
        if (Client == null) return Task.CompletedTask;
        Client.Close();
        Client.Opened -= () => Opened?.Invoke();
        Client.Closed -= () => Closed?.Invoke();
        Client = default;
        return Task.CompletedTask;
    }


    /// <inheritdoc />
    public virtual void Dispose() {
        Close();
        Client?.Dispose();
    }
}