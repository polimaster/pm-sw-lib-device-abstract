using System;
using System.Linq;
using System.Threading;
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
        var stream = new Mock<IMyDeviceStream>();
        client.Setup(e => e.GetStream()).Returns(stream.Object);
        transport.Setup(e => e.Client).Returns(client.Object);
        transport.Setup(e => e.ExecOnStream(It.IsAny<Func<IMyDeviceStream, Task>>(), Token))
            .Returns<Func<IMyDeviceStream, Task>, CancellationToken>(async (func, _) => {
                await func(stream.Object);
            });

        var dev = new MyDevice(transport.Object, SETTING_DESCRIPTORS, LOGGER_FACTORY);
        var settingsCount = dev.GetSettings().Count();

        await dev.ReadAllSettings(Token);

        stream.Verify(s => s.Write(It.IsAny<byte[]>(), Token), Times.Exactly(settingsCount));
    }

    [Fact]
    public async Task ShouldWriteSettings() {
        var transport = new Mock<IMyTransport>();
        var client = new Mock<IClient<IMyDeviceStream>>();
        var stream = new Mock<IMyDeviceStream>();
        client.Setup(e => e.GetStream()).Returns(stream.Object);
        transport.Setup(e => e.Client).Returns(client.Object);
        transport.Setup(e => e.ExecOnStream(It.IsAny<Func<IMyDeviceStream, Task>>(), Token))
            .Returns<Func<IMyDeviceStream, Task>, CancellationToken>(async (func, _) => {
                await func(stream.Object);
            });

        var dev = new MyDevice(transport.Object, SETTING_DESCRIPTORS, LOGGER_FACTORY);

        var v = TimeSpan.FromSeconds(10);
        dev.HistoryInterval.Value = v;
        dev.StringSetting.Value = "test";
        await dev.WriteAllSettings(Token);

        // should write changed values: HistoryInterval and StringSetting
        stream.Verify(s => s.Write(It.IsAny<byte[]>(), Token), Times.Exactly(2));
    }


    [Fact]
    public void ShouldGetSetting() {
        var transport = new Mock<IMyTransport>();
        var dev = new MyDevice(transport.Object, SETTING_DESCRIPTORS, LOGGER_FACTORY);

        var setting = dev.GetSetting(SETTING_DESCRIPTORS.HistoryIntervalSettingDescriptor);

        Assert.NotNull(setting);
        Assert.Equal(SETTING_DESCRIPTORS.HistoryIntervalSettingDescriptor, setting.Descriptor);
    }

    [Fact]
    public void ShouldSetSetting() {
        var transport = new Mock<IMyTransport>();
        var dev = new MyDevice(transport.Object, SETTING_DESCRIPTORS, LOGGER_FACTORY);

        var setting = dev.SetSetting(SETTING_DESCRIPTORS.HistoryIntervalSettingDescriptor, new TimeSpan(0, 1, 0));
        Assert.NotNull(setting);
    }

    [Fact]
    public void ShouldThrowExceptionWhenSettingTypeMismatch() {
        var transport = new Mock<IMyTransport>();
        var dev = new MyDevice(transport.Object, SETTING_DESCRIPTORS, LOGGER_FACTORY);

        Exception? ex = null;

        try {
            dev.SetSetting(SETTING_DESCRIPTORS.HistoryIntervalSettingDescriptor, "this is a string but should be TimeSpan");
        } catch (ArgumentException e) {
            ex = e;
        }

        Assert.NotNull(ex);
    }

}