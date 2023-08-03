using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract;

/// <inheritdoc />
public abstract class ATransportDiscovery<TConnectionParams> : ITransportDiscovery<TConnectionParams> {
    /// <summary>
    /// 
    /// </summary>
    protected readonly IClientFactory ClientFactory;
    /// <summary>
    /// 
    /// </summary>
    protected readonly ILoggerFactory? LoggerFactory;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="factory"></param>
    /// <param name="loggerFactory"></param>
    protected ATransportDiscovery(IClientFactory factory, ILoggerFactory? loggerFactory = null) {
        ClientFactory = factory;
        LoggerFactory = loggerFactory;
    }

    /// <inheritdoc />
    public abstract void Start(CancellationToken token);

    /// <inheritdoc />
    public abstract void Stop();

    /// <inheritdoc />
    public Action<IEnumerable<ITransport<TConnectionParams>>>? Found { get; set; }

    /// <inheritdoc />
    public Action<IEnumerable<ITransport<TConnectionParams>>>? Lost { get; set; }

    /// <inheritdoc />
    public abstract void Dispose();
}