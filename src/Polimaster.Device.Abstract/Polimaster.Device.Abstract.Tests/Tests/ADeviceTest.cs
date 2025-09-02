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
        var dev1 = new MyDevice(transport.Object, SETTING_DESCRIPTORS, LOGGER_FACTORY);
        var dev2 = new MyDevice(transport.Object, SETTING_DESCRIPTORS, LOGGER_FACTORY);

        transport.Setup(e => e.Client).Returns(client.Object);
        
        Assert.Equal(dev1.Id, dev2.Id);
        Assert.True(dev1.Equals(dev2));
    }

    [Fact]
    public void ShouldDispose() {
        var transport = new Mock<IMyTransport>();
        transport.Setup(e => e.Client).Returns(new Mock<IClient<IMyDeviceStream>>().Object);
        var dev = new MyDevice(transport.Object, SETTING_DESCRIPTORS, LOGGER_FACTORY);

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

        var dev = new MyDevice(transport.Object, SETTING_DESCRIPTORS, LOGGER_FACTORY);
        var settingsCount = dev.GetSettings().Count();

        await dev.ReadAllSettings(Token);

        transport.Verify(e => e.ExecOnStream(It.IsAny<Func<IMyDeviceStream,Task>>(), Token), Times.Exactly(settingsCount));
    }

    [Fact]
    public async Task ShouldWriteSettings() {
        var transport = new Mock<IMyTransport>();
        var client = new Mock<IClient<IMyDeviceStream>>();
        transport.Setup(e => e.Client).Returns(client.Object);

        var dev = new MyDevice(transport.Object, SETTING_DESCRIPTORS, LOGGER_FACTORY);

        var v = TimeSpan.FromSeconds(10);
        dev.HistoryInterval.Value = v;
        dev.StringSetting.Value = "test";
        await dev.WriteAllSettings(Token);

        // should write changed values: HistoryInterval and StringSetting
        transport.Verify(e => e.ExecOnStream(It.IsAny<Func<IMyDeviceStream,Task>>(), Token), Times.Exactly(2));
    }

}