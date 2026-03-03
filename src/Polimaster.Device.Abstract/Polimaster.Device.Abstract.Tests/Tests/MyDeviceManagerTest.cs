using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Tests.Impl.Device;
using Polimaster.Device.Abstract.Tests.Impl.Settings;
using Polimaster.Device.Abstract.Tests.Impl.Transport;

namespace Polimaster.Device.Abstract.Tests.Tests; 

public class MyDeviceManagerTest : Mocks {

    [Fact]
    public void ShouldTrackDiscovery() {
        var disco = new Mock<IMyDeviceDiscovery>();
        var manager = new MyDeviceManager(disco.Object, LOGGER_FACTORY);

        IMyDevice? devAttached = null;
        manager.Attached += device => {
            devAttached = device;
        };

        var p = new ClientParams(1, 1);
        var client = new MyClient(p, null);

        var transport = new Mock<IMyTransport>();
        transport.Setup(e => e.Client).Returns(client);
        var list = new List<ClientParams> { new(1, 1) };

        disco.Raise(e => e.Found += null, list);
        Assert.Single(manager.GetDevices());
        Assert.True(devAttached?.HasSame(transport.Object));

        IMyDevice? devRemoved = null;
        manager.Removed += device => {
            devRemoved = device;
        };

        disco.Raise(e => e.Lost += null, list);
        Assert.Empty(manager.GetDevices());
        Assert.True(devRemoved?.HasSame(transport.Object));
    }

    [Fact]
    public void ShouldAddAndRemoveDevice() {
        var manager = new MySimpleDeviceManager(LOGGER_FACTORY);
        var transport = new Mock<IMyTransport>();
        var dev = new MyDevice(transport.Object, SETTING_DESCRIPTORS, LOGGER_FACTORY);

        manager.Add(dev);
        Assert.Single(manager.GetDevices());
        Assert.Contains(dev, manager.GetDevices());

        manager.Remove(dev);
        Assert.Empty(manager.GetDevices());
    }

    private class MySimpleDeviceManager : ADeviceManager<IMyDevice> {
        public MySimpleDeviceManager(ILoggerFactory? loggerFactory) : base(loggerFactory) {
        }

        public override event Action<IMyDevice>? Attached;
        public override event Action<IMyDevice>? Removed;
        public override ISettingDescriptors SettingsDescriptors => new MySettingDescriptors();

        public void Add(IMyDevice dev) => AddDevice(dev);
        public void Remove(IMyDevice dev) => RemoveDevice(dev);
    }
    
}