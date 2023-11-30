using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device; 

public class MyDeviceManager : ADeviceManager<IMyDeviceDiscovery, MyDevice> {

    protected override void OnLost(IEnumerable<ITransport> transports) {
        var lost = transports.Select(transport => new MyDevice(transport, LoggerFactory)).ToList();
        var toRemove = Devices.Where(x => lost.All(y => y != x)).ToList();
        
        Devices.RemoveAll(x => toRemove.All(y => y == x));
        foreach (var dev in toRemove) Removed(dev);
        return;

        void Removed(MyDevice dev) {
            this.Removed?.Invoke(dev);
            dev.Dispose();
        }
    }

    protected override void OnFound(IEnumerable<ITransport> transports) {
        foreach (var transport in transports) {
            var dev = new MyDevice(transport, LoggerFactory);
            Devices.Add(dev);
            Attached?.Invoke(dev);
        }
    }

    public MyDeviceManager(IMyDeviceDiscovery discovery, ILoggerFactory? loggerFactory) : base(discovery, loggerFactory) {
    }

    public override event Action<MyDevice>? Attached;
    public override event Action<MyDevice>? Removed;
}