using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Polimaster.Device.Abstract.Tests.Impl.Device;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Tests; 

public class MyDeviceTest : Mocks {

    [Fact]
    public void ShouldTrackId() {
        var guid = Guid.NewGuid().ToString();
        var client = new Mock<IClient<string>>();
        client.Setup(e => e.ConnectionId).Returns(guid);

        var transport = new Mock<ITransport<string>>();
        var dev1 = new MyDevice(transport.Object, LOGGER_FACTORY);
        var dev2 = new MyDevice(transport.Object, LOGGER_FACTORY);

        transport.Setup(e => e.Client).Returns(client.Object);
        
        Assert.Equal(dev1.Id, dev2.Id);
        Assert.True(dev1.Equals(dev2));
    }

    [Fact]
    public void ShouldDispose() {
        var transport = new Mock<ITransport<string>>();
        transport.Setup(e => e.Client).Returns(new Mock<IClient<string>>().Object);
        var dev = new MyDevice(transport.Object, LOGGER_FACTORY);

        var check = false;
        dev.IsDisposing += () => check = true;
        
        dev.Dispose();
        
        transport.Verify(e => e.Dispose());
        Assert.True(check);
    }

    [Fact]
    public async Task ShouldExecute() {
        var transport = new Mock<ITransport<string>>();
        var dev = new MyDevice(transport.Object, LOGGER_FACTORY);
        
        await dev.GetTime(Token);
        
        transport.Verify(e => e.ReadAsync(Token));
    }

    [Fact]
    public async Task ShouldCatchExceptionOnExecute() {
        var transport = new Mock<ITransport<string>>();
        var exception = new Exception();
        transport.Setup(e => e.ReadAsync(Token)).ThrowsAsync(exception);
        var dev = new MyDevice(transport.Object, LOGGER_FACTORY);

        Exception? ex = null;

        try {
            await dev.GetTime(Token);
        } catch (Exception e) {
            ex = e;
        }

        Assert.Equal(exception, ex);
    }
    

    [Fact]
    public async Task ShouldReadInfo() {
        var transport = new Mock<ITransport<string>>();
        var dev = new MyDevice(transport.Object, LOGGER_FACTORY);
        
        Assert.Null(dev.DeviceInfo);
        
        await dev.ReadDeviceInfo(CancellationToken.None);

        Assert.NotNull(dev.DeviceInfo);
        transport.Verify(e => e.ReadAsync(It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async Task ShouldReadSettings() {
        var transport = new Mock<ITransport<string>>();
        transport.Setup(e => e.Client).Returns(new Mock<IClient<string>>().Object);
        var dev = new MyDevice(transport.Object, LOGGER_FACTORY);

        const ushort v = 10;
        transport.Setup(e => e.ReadAsync(Token)).Returns(Task.FromResult(v.ToString()));
        
        await dev.HistoryInterval.Read(Token);
        transport.Verify(e => e.ReadAsync(Token), Times.Exactly(1));
        
        // should not call Read if setting IsSynchronized
        await dev.HistoryInterval.Read(Token);
        transport.Verify(e => e.ReadAsync(Token), Times.Exactly(1));

    }

    [Fact]
    public async Task ShouldWriteSettings() {
        var transport = new Mock<ITransport<string>>();
        transport.Setup(e => e.Client).Returns(new Mock<IClient<string>>().Object);
        var dev = new MyDevice(transport.Object, LOGGER_FACTORY);
        
        const ushort v = 10;
        dev.HistoryInterval.Value = v;
        await dev.WriteAllSettings(Token);
        
        // should write changed value
        transport.Verify(e => e.WriteAsync("CMD=INTERVAL:10", Token));
        
        // should NOT write because no changes
        transport.Verify(e => e.WriteAsync("CMD=INTERVAL:1", Token), Times.Never);
        
    }
    
}