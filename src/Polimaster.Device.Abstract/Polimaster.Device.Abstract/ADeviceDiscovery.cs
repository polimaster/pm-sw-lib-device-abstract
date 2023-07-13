using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract;

public abstract class ATransportDiscovery<TConnectionParams> : ITransportDiscovery<TConnectionParams> {
    protected readonly IClientFactory ClientFactory;
    protected readonly ILoggerFactory? LoggerFactory;

    protected ATransportDiscovery(IClientFactory factory, ILoggerFactory? loggerFactory = null) {
        ClientFactory = factory;
        LoggerFactory = loggerFactory;
    }

    public abstract Task Start(CancellationToken token);

    public Action<IEnumerable<ITransport<TConnectionParams>>>? Found { get; set; }
    public Action<IEnumerable<ITransport<TConnectionParams>>>? Lost { get; set; }

}