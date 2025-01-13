using System.Collections.Generic;
using System.Threading;
using Polimaster.Device.Abstract.Tests.Impl.Device;
using Polimaster.Device.Abstract.Tests.Impl.Device.Transport;

namespace Polimaster.Device.Abstract.Tests.Tests;

public class MyDeviceDiscoveryTest : Mocks {
    [Fact]
    public void ShouldSearch() {
        var disco = new MyDeviceDiscovery(LOGGER_FACTORY);

        IEnumerable<ClientParams>? found = null;
        IEnumerable<ClientParams>? lost = null;
        disco.Found += transports => { found = transports; };
        disco.Lost += transports => { lost = transports; };
        disco.Start(Token);

        // wait 1 sec
        Thread.Sleep(1000);

        Assert.NotNull(found);
        Assert.NotNull(lost);
    }

    [Fact]
    public void ShouldStop() {
        var disco = new MyDeviceDiscovery(LOGGER_FACTORY);

        var count = 0;
        disco.Found += _ => { count++; };
        disco.Start(Token);
        
        // wait 1 sec
        Thread.Sleep(1000);
        disco.Stop();
        var calls = count;

        // wait more 1 sec
        Thread.Sleep(1000);
        var moreCalls = count;
        
        Assert.Equal(calls, moreCalls);
    }
}