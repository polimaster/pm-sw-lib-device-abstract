using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport.Interfaces;

namespace Polimaster.Device.Abstract;

public abstract class ATransportDiscovery<TData, TConnectionParams> : ITransportDiscovery<TData, TConnectionParams> {
    protected readonly IClientFactory ClientFactory;
    protected readonly ILoggerFactory? LoggerFactory;

    protected ATransportDiscovery(IClientFactory factory, ILoggerFactory? loggerFactory = null) {
        ClientFactory = factory;
        LoggerFactory = loggerFactory;
    }

    public abstract Task Search(CancellationToken token = new());

    public Action<IEnumerable<ITransport<TData, TConnectionParams>>>? Found { get; set; }
    public Action<IEnumerable<ITransport<TData, TConnectionParams>>>? Lost { get; set; }

}