using System;
using System.Collections.Generic;
using Moq;
using Polimaster.Device.Abstract.Tests.Impl.Device;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Tests; 

public class MyDeviceManagerTest : Mocks {

    [Fact]
    public void ShouldTrackDiscovery() {
        var disco = new Mock<IMyDeviceDiscovery>();
        var manager = new MyDeviceManager(disco.Object, LOGGER_FACTORY);

        string? devAttached = null;
        manager.Attached += device => {
            devAttached = device.Id;
        };
        
        var transport = new Mock<ITransport>();
        var connectionId = Guid.NewGuid().ToString();
        transport.Setup(e => e.ConnectionId).Returns(connectionId);
        var list = new List<ITransport> { transport.Object };
        
        disco.Raise(e => e.Found += null, list);
        Assert.Equal(list.Count, manager.Devices.Count);
        Assert.Equal(connectionId, devAttached);
        
        
        string? devRemoved = null;
        manager.Removed += device => {
            devRemoved = device.Id;
        };
        
        disco.Raise(e => e.Lost += null, list);
        Assert.Empty(manager.Devices);
        Assert.Equal(connectionId, devRemoved);
        
    }
    
}