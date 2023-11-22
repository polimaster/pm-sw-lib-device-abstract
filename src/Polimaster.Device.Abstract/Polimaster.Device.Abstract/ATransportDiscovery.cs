using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract;

/// <inheritdoc />
public abstract class ATransportDiscovery : ITransportDiscovery {
    /// <summary>
    /// 
    /// </summary>
    protected readonly ILoggerFactory? LoggerFactory;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="loggerFactory"></param>
    protected ATransportDiscovery(ILoggerFactory? loggerFactory) {
        LoggerFactory = loggerFactory;
    }

    /// <inheritdoc />
    public abstract void Start(CancellationToken token);

    /// <inheritdoc />
    public abstract void Stop();

    /// <inheritdoc />
    public Action<IEnumerable<ITransport>>? Found { get; set; }

    /// <inheritdoc />
    public Action<IEnumerable<ITransport>>? Lost { get; set; }

    /// <inheritdoc />
    public abstract void Dispose();
}