using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Polimaster.Device.Abstract.Device;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Tests.Impl.Device;
using Polimaster.Device.Abstract.Tests.Impl.Device.Commands;
using Polimaster.Device.Abstract.Tests.Impl.Device.Settings;
using Polimaster.Device.Abstract.Tests.Tests.Commands;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Tests; 

public class MyDeviceTest : Mocks {

    [Fact]
    public void ShouldTrackId() {
        var transport = new Mock<ITransport>();
        var dev1 = new MyDevice(transport.Object, LOGGER_FACTORY);
        var dev2 = new MyDevice(transport.Object, LOGGER_FACTORY);

        transport.Setup(e => e.ConnectionId).Returns(Guid.NewGuid().ToString);
        
        Assert.Equal(dev1.Id, dev2.Id);
        Assert.True(dev1.Equals(dev2));
    }

    [Fact]
    public void ShouldDispose() {
        var transport = new Mock<ITransport>();
        var dev = new MyDevice(transport.Object, LOGGER_FACTORY);

        var check = false;
        dev.IsDisposing += () => check = true;
        
        dev.Dispose();
        
        transport.Verify(e => e.Dispose());
        Assert.True(check);
    }

    [Fact]
    public async void ShouldExecute() {
        var transport = new Mock<ITransport>();
        var f = new Mock<Func<ITransport, Task>>();
        var dev = new MyDevice(transport.Object, LOGGER_FACTORY);
        
        await dev.Execute(f.Object);
        
        transport.Verify(e => e.OpenAsync());
        transport.Verify(e => e.Close());
        
        f.Verify(e => e.Invoke(transport.Object));
    }
    

    [Fact]
    public async void ShouldReadInfo() {
        var transport = new Mock<ITransport>();
        var dev = new MyDevice(transport.Object, LOGGER_FACTORY);
        
        Assert.Null(dev.DeviceInfo);
        
        await dev.ReadDeviceInfo();

        Assert.NotNull(dev.DeviceInfo);
        transport.Verify(e => e.Read(It.IsAny<IDataReader<DeviceInfo>>(), It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async void ShouldReadSettings() {
        var transport = new Mock<ITransport>();
        var dev = new MyDevice(transport.Object, LOGGER_FACTORY);

        ushort? v = 10;
        transport.Setup(e => e.Read(It.IsAny<HistoryIntervalReader>(), Token)).Returns(Task.FromResult(v));
        await dev.ReadAllSettings(Token);
        Assert.Equal(v, dev.HistoryInterval.Value);

        ushort? v2 = 20;
        transport.Setup(e => e.Read(It.IsAny<HistoryIntervalReader>(), Token)).Returns(Task.FromResult(v2));
        await dev.HistoryInterval.Read(transport.Object, Token);
        Assert.Equal(v2, dev.HistoryInterval.Value);
    }

    [Fact]
    public async void ShouldWriteSettings() {
        var transport = new Mock<ITransport>();
        var dev = new MyDevice(transport.Object, LOGGER_FACTORY);
        
        ushort? v = 10;
        dev.HistoryInterval.Value = v;
        await dev.WriteAllSettings(Token);
        
        // should write changed value
        transport.Verify(e => e.Write(It.IsAny<HistoryIntervalWriter>(), v, Token));
        
        // should NOT write because no changes
        transport.Verify(e => e.Write(It.IsAny<MyParamWriter>(), It.IsAny<MyParam>(), Token), Times.Never);
        
    }
    
}