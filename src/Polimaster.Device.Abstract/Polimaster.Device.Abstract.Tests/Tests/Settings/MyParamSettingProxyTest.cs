using System;
using System.Threading.Tasks;
using Moq;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Tests.Impl.Settings;

namespace Polimaster.Device.Abstract.Tests.Tests.Settings; 

public class ADeviceSettingProxyTest : Mocks {

    // [Fact]
    // public void ShouldHaveDefaultDescriptor() {
    //     var transport = new Mock<IMyTransport>();
    //     var client = new Mock<IClient<IMyDeviceStream>>();
    //     var stream = new Mock<IMyDeviceStream>();
    //     client.Setup(e => e.GetStream()).Returns(stream.Object);
    //     transport.Setup(e => e.Client).Returns(client.Object);
    //
    //     var setting = new MyParamSetting(transport.Object, SETTING_DESCRIPTORS, LOGGER_FACTORY);
    //
    //     var proxyDescriptor = new SettingDescriptor("MyParamSettingProxyTest", typeof(string), SettingAccessLevel.EXTENDED, "MyParamSettingGroup");
    //     var proxy = new MyParamSettingProxy(setting, proxyDescriptor);
    //
    //     Assert.NotNull(proxy.Descriptor.Name);
    //     Assert.NotNull(proxy.Descriptor.GroupName);
    //     Assert.Equal(SettingAccessLevel.EXTENDED, proxy.Descriptor.AccessLevel);
    // }

    // [Fact]
    // public void ShouldHaveValidDescriptor() {
    //     var reader = new Mock<IDataReader<MyParam>>();
    //     var descriptor = new SettingDescriptor("test", typeof(MyParam));
    //     var setting = new MyParamSetting(reader.Object, descriptor);
    //
    //     var proxyDescriptor = new SettingDescriptor("MyParamSettingProxyTest", typeof(string), SettingAccessLevel.EXTENDED, "MyParamSettingGroup");
    //     var proxy = new MyParamSettingProxy(setting, proxyDescriptor);
    //
    //     Assert.Equal(proxyDescriptor.Name, proxy.Descriptor.Name);
    //     Assert.Equal(proxyDescriptor.GroupName, proxy.Descriptor.GroupName);
    //     Assert.Equal(proxyDescriptor.AccessLevel, proxy.Descriptor.AccessLevel);
    // }


    [Fact]
    public async Task ShouldSetProperty() {

        var reader = new Mock<IDataReader<MyParam>>();
        var writer = new Mock<IDataWriter<MyParam>>();
        reader.Setup(e => e.Read(Token)).ReturnsAsync(new MyParam());

        var setting = new MyParamSetting(new SettingDefinition<MyParam> {
            Reader = reader.Object,
            Writer = writer.Object,
            Descriptor = SETTING_DESCRIPTORS.MyParamSettingDescriptor,
        });

        var proxy = new MyParamSettingProxy(setting, SETTING_DESCRIPTORS.StringSettingDescriptor);
        await setting.Read(Token);

        Assert.False(setting.IsDirty);
        Assert.False(proxy.IsDirty);
        Assert.True(setting.IsSynchronized);
        Assert.True(proxy.IsSynchronized);

        proxy.Value = "test";

        Assert.True(setting.IsDirty);
        Assert.True(proxy.IsDirty);
        Assert.Null(proxy.Exception);
    }
    
    [Fact]
    public async Task ShouldValidateValue() {
        var reader = new Mock<IDataReader<MyParam>>();
        var setting = new MyParamSetting(new SettingDefinition<MyParam> {
            Reader = reader.Object,
            Descriptor = SETTING_DESCRIPTORS.MyParamSettingDescriptor,
        });
        await setting.Read(Token);

        var proxy = new MyParamSettingProxy(setting, SETTING_DESCRIPTORS.StringSettingDescriptor) {
            Value = MyParamSettingProxy.FORBIDDEN_VALUES[0]
        };

        Assert.True(proxy.IsDirty);
        Assert.False(proxy.IsValid);

        proxy = new MyParamSettingProxy(setting, SETTING_DESCRIPTORS.StringSettingDescriptor) {
            Value = null
        };

        Assert.True(proxy.IsDirty);
        Assert.False(proxy.IsValid);
    }
    
    [Fact]
    public async Task ShouldRead() {
        var setting = new Mock<IDeviceSetting<MyParam>>();
        var proxy = new MyParamSettingProxy(setting.Object, SETTING_DESCRIPTORS.StringSettingDescriptor);

        await proxy.Read(Token);
        setting.Verify(e => e.Read(Token));
    }
    
    [Fact]
    public async Task ShouldWrite() {
        var reader = new Mock<IDataReader<MyParam>>();
        reader.Setup(e => e.Read(Token)).ReturnsAsync(new MyParam());
        var writer = new Mock<IDataWriter<MyParam>>();

        var setting = new MyParamSetting(new SettingDefinition<MyParam> {
            Reader = reader.Object,
            Writer = writer.Object,
            Descriptor = SETTING_DESCRIPTORS.MyParamSettingDescriptor,
        });
        await setting.Read(Token);

        var proxy = new MyParamSettingProxy(setting, SETTING_DESCRIPTORS.StringSettingDescriptor) { Value = "value" };

        await proxy.CommitChanges(Token);
        writer.Verify(e => e.Write(It.IsAny<MyParam>(), Token));
        Assert.False(proxy.IsDirty);
    }
    
    
    [Fact]
    public async Task ShouldNotWriteValue() {
        var reader = new Mock<IDataReader<MyParam>>();
        var writer = new Mock<IDataWriter<MyParam>>();
        var setting = new MyParamSetting(new SettingDefinition<MyParam> {
            Reader = reader.Object,
            Writer = writer.Object,
            Descriptor = SETTING_DESCRIPTORS.MyParamSettingDescriptor,
        });
        await setting.Read(Token);
        var proxy = new MyParamSettingProxy(setting, SETTING_DESCRIPTORS.StringSettingDescriptor) { Value = MyParamSettingProxy.FORBIDDEN_VALUES[0] };

        await proxy.CommitChanges(Token);

        Assert.True(proxy.IsDirty);
        Assert.False(proxy.IsValid);

        writer.Verify(e => e.Write(It.IsAny<MyParam>(), Token), Times.Never);

        Assert.True(proxy.IsDirty);
        Assert.False(proxy.IsValid);
    }

    [Fact]
    public void ShouldCheckReadOnly() {
        var reader = new Mock<IDataReader<MyParam>>();
        var setting = new MyParamSetting(new SettingDefinition<MyParam> {
            Reader = reader.Object,
            Descriptor = SETTING_DESCRIPTORS.MyParamSettingDescriptor,
        });

        var proxy = new MyParamSettingProxy(setting, SETTING_DESCRIPTORS.StringSettingDescriptor);

        Assert.True(proxy.ReadOnly);
    }
    
    
    [Fact]
    public async Task ShouldCatchExceptionWhileWrite() {
        var reader = new Mock<IDataReader<MyParam>>();
        reader.Setup(e => e.Read(Token)).ReturnsAsync(new MyParam {Value = "value from device"});
        var writer = new Mock<IDataWriter<MyParam>>();

        var setting = new MyParamSetting(new SettingDefinition<MyParam> {
            Reader = reader.Object,
            Writer = writer.Object,
            Descriptor = SETTING_DESCRIPTORS.MyParamSettingDescriptor,
        });
        await setting.Read(Token);

        var proxy = new MyParamSettingProxy(setting, SETTING_DESCRIPTORS.StringSettingDescriptor) {
            Value = "test"
        };

        Assert.True(proxy.IsDirty);

        var ex = new Exception();
        writer.Setup(e => e.Write(It.IsAny<MyParam>(), Token)).ThrowsAsync(ex);

        await proxy.CommitChanges(Token);

        Assert.Equal(ex, proxy.Exception);
        Assert.True(proxy.IsError);
        Assert.True(proxy.IsDirty);
    }
    
    
    [Fact]
    public async Task ShouldCatchExceptionWhileRead() {
        var reader = new Mock<IDataReader<MyParam>>();
        var ex = new Exception();
        reader.Setup(e => e.Read(Token)).ThrowsAsync(ex);

        var setting = new MyParamSetting(new SettingDefinition<MyParam> {
            Reader = reader.Object,
            Descriptor = SETTING_DESCRIPTORS.MyParamSettingDescriptor,
        });

        var proxy = new MyParamSettingProxy(setting, SETTING_DESCRIPTORS.StringSettingDescriptor);

        try {
            await proxy.Read(Token);
        } catch (Exception) { // ignored
        }


        Assert.Equal(ex, proxy.Exception);
        Assert.True(proxy.IsError);
    }
    
}