﻿using System.Collections.Generic;
using Moq;
using Polimaster.Device.Abstract.Tests.Impl.Device;
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
        var list = new List<ClientParams> { new(1,1) };
        
        disco.Raise(e => e.Found += null, list);
        Assert.Equal(list.Count, manager.Devices.Count);
        Assert.True(devAttached?.HasSame(transport.Object));
        
        IMyDevice? devRemoved = null;
        manager.Removed += device => {
            devRemoved = device;
        };
        
        disco.Raise(e => e.Lost += null, list);
        Assert.Empty(manager.Devices);
        Assert.True(devRemoved?.HasSame(transport.Object));
        
    }
    
}