using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport.Interfaces;

namespace Polimaster.Device.Abstract;

public abstract class ADeviceDiscovery<TData, TConnectionParams> : IDeviceDiscovery<TData, TConnectionParams> {
    protected readonly IClientFactory ClientFactory;
    protected readonly ILoggerFactory? LoggerFactory;

    protected ADeviceDiscovery(IClientFactory factory, ILoggerFactory? loggerFactory = null) {
        ClientFactory = factory;
        LoggerFactory = loggerFactory;
    }

    public abstract IEnumerable<ITransport<TData, TConnectionParams>> Search();
}