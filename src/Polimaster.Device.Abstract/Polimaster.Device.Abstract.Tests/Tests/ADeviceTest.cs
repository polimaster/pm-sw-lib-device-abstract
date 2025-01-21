using System;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Polimaster.Device.Abstract.Tests.Impl.Device;
using Polimaster.Device.Abstract.Tests.Impl.Transport;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Tests; 

public class MyDeviceTest : Mocks {

    [Fact]
    public void ShouldTrackId() {
        var guid = Guid.NewGuid().ToString();
        var client = new Mock<IClient<IMyDeviceStream>>();
        client.Setup(e => e.ConnectionId).Returns(guid);

        var transport = new Mock<IMyTransport>();
        var dev1 = new MyDevice(transport.Object, LOGGER_FACTORY);
        var dev2 = new MyDevice(transport.Object, LOGGER_FACTORY);

        transport.Setup(e => e.Client).Returns(client.Object);
        
        Assert.Equal(dev1.Id, dev2.Id);
        Assert.True(dev1.Equals(dev2));
    }

    [Fact]
    public void ShouldDispose() {
        var transport = new Mock<IMyTransport>();
        transport.Setup(e => e.Client).Returns(new Mock<IClient<IMyDeviceStream>>().Object);
        var dev = new MyDevice(transport.Object, LOGGER_FACTORY);

        var check = false;
        dev.IsDisposing += () => check = true;
        
        dev.Dispose();
        
        transport.Verify(e => e.Dispose());
        Assert.True(check);
    }

    [Fact]
    public async Task ShouldReadAllSettings() {
        var transport = new Mock<IMyTransport>();
        var client = new Mock<IClient<IMyDeviceStream>>();
        transport.Setup(e => e.Client).Returns(client.Object);

        var dev = new MyDevice(transport.Object, LOGGER_FACTORY);
        var settingsCount = dev.GetSettings().Count();

        await dev.ReadAllSettings(Token);

        transport.Verify(e => e.ExecOnStream(It.IsAny<Func<IMyDeviceStream,Task>>(), Token), Times.Exactly(settingsCount));
    }

    [Fact]
    public async Task ShouldWriteSettings() {
        var transport = new Mock<IMyTransport>();
        var client = new Mock<IClient<IMyDeviceStream>>();
        transport.Setup(e => e.Client).Returns(client.Object);

        var dev = new MyDevice(transport.Object, LOGGER_FACTORY);

        const ushort v = 10;
        dev.HistoryInterval.Value = v;
        dev.StringSetting.Value = "test";
        await dev.WriteAllSettings(Token);

        // should write changed values: HistoryInterval and StringSetting
        transport.Verify(e => e.ExecOnStream(It.IsAny<Func<IMyDeviceStream,Task>>(), Token), Times.Exactly(2));
    }



    // [Fact]
    // public async Task ShouldThrowExceptionOnExecute() {
    //     var transport = new Mock<IMyTransport>();
    //     var exception = new Exception();
    //     transport.Setup(e => e.ExecOnStream(It.IsAny<Func<IMyDeviceStream, Task>>(), Token)).ThrowsAsync(exception);
    //     var dev = new MyDevice(transport.Object, LOGGER_FACTORY);
    //
    //     Exception? ex = null;
    //
    //     try {
    //         await dev.GetTime(Token);
    //     } catch (Exception e) {
    //         ex = e;
    //     }
    //
    //     Assert.Equal(exception, ex);
    // }
    //
    // [Fact]
    // public async Task ShouldExecute() {
    //     var transport = new Mock<IMyTransport>();
    //     var dev = new MyDevice(transport.Object, LOGGER_FACTORY);
    //
    //     await dev.GetTime(Token);
    //
    //     transport.Verify(e => e.ExecOnStream(It.IsAny<Func<IMyDeviceStream, Task>>(), Token));
    // }
    

    // [Fact]
    // public async Task ShouldReadInfo() {
    //     var transport = new Mock<IMyTransport>();
    //     var client = new Mock<IClient<IMyDeviceStream>>();
    //     var stream = new Mock<IMyDeviceStream>();
    //     client.Setup(e => e.GetStream()).Returns(stream.Object);
    //     transport.Setup(e => e.Client).Returns(client.Object);
    //
    //     var dev = new MyDevice(transport.Object, LOGGER_FACTORY);
    //
    //     Assert.Null(dev.DeviceInfo);
    //
    //     await dev.ReadDeviceInfo(CancellationToken.None);
    //
    //     Assert.NotNull(dev.DeviceInfo);
    //     stream.Verify(e => e.Read(Token));
    //     // transport.Verify(e => e.Read(It.IsAny<CancellationToken>()));
    // }

    // [Fact]
    // public async Task ShouldReadSettings() {
    //     var transport = new Mock<IMyTransport>();
    //     var client = new Mock<IClient<IMyDeviceStream>>();
    //     var stream = new Mock<IMyDeviceStream>();
    //     client.Setup(e => e.GetStream()).Returns(stream.Object);
    //     transport.Setup(e => e.Client).Returns(client.Object);
    //
    //     var dev = new MyDevice(transport.Object, LOGGER_FACTORY);
    //
    //     const ushort v = 10;
    //     stream.Setup(e => e.Read(Token)).Returns(Task.FromResult(BitConverter.GetBytes(v)));
    //     // transport.Setup(e => e.ReadAsync(Token)).Returns(Task.FromResult(BitConverter.GetBytes(v)));
    //
    //     await dev.HistoryInterval.Read(Token);
    //     stream.Verify(e => e.Read(Token), Times.Exactly(1));
    //     // transport.Verify(e => e.ReadAsync(Token), Times.Exactly(1));
    //
    //     // should not call Read if setting IsSynchronized
    //     await dev.HistoryInterval.Read(Token);
    //     // transport.Verify(e => e.ReadAsync(Token), Times.Exactly(1));
    //     stream.Verify(e => e.Read(Token), Times.Exactly(1));
    // }


    
}