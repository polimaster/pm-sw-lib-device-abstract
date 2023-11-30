using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Polimaster.Device.Abstract.Tests.Impl.Device;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Tests;

public class MyDeviceDiscoveryTest : Mocks {
    [Fact]
    public void ShouldSearch() {
        var disco = new MyDeviceDiscovery(LOGGER_FACTORY);

        IEnumerable<ITransport>? found = null;
        IEnumerable<ITransport>? lost = null;
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
        disco.Found += transports => { count++; };
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