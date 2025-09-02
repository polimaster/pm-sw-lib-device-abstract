using System;
using System.Threading.Tasks;
using Moq;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Tests.Impl.Commands;
using Polimaster.Device.Abstract.Tests.Impl.Settings;
using Polimaster.Device.Abstract.Tests.Impl.Transport;
using Polimaster.Device.Abstract.Transport;

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
        var transport = new Mock<IMyTransport>();

        var setting = new MyParamSetting(transport.Object, SETTING_DESCRIPTORS, LOGGER_FACTORY) {
            Value = new MyParam()
        };
        // await setting.Read(Token);

        var proxyDescriptor = new SettingDescriptor("test1", typeof(string));
        var proxy = new MyParamSettingProxy(setting, proxyDescriptor) {
            Value = "test"
        };

        Assert.True(proxy.IsDirty);
        Assert.Null(proxy.Exception);
    }
    
    [Fact]
    public async Task ShouldValidateValue() {
        var transport = new Mock<IMyTransport>();

        var setting = new MyParamSetting(transport.Object, SETTING_DESCRIPTORS, LOGGER_FACTORY) {
            Value = new MyParam()
        };
        // await setting.Read(Token);

        var proxyDescriptor = new SettingDescriptor("test1", typeof(string));
        var proxy = new MyParamSettingProxy(setting, proxyDescriptor) {
            Value = MyParamSettingProxy.FORBIDDEN_VALUES[0]
        };
        
        Assert.True(proxy.IsDirty);
        Assert.False(proxy.IsValid);
        
        proxy = new MyParamSettingProxy(setting, proxyDescriptor) {
            Value = null
        };
        
        Assert.True(proxy.IsDirty);
        Assert.False(proxy.IsValid);
    }
    
    [Fact]
    public async Task ShouldRead() {
        var setting = new Mock<IDeviceSetting<MyParam>>();
        var proxyDescriptor = new SettingDescriptor("test1", typeof(string));
        var proxy = new MyParamSettingProxy(setting.Object, proxyDescriptor);

        await proxy.Read(Token);
        setting.Verify(e => e.Read(Token));
    }
    
    [Fact]
    public async Task ShouldWrite() {
        var transport = new Mock<IMyTransport>();

        var setting = new MyParamSetting(transport.Object, SETTING_DESCRIPTORS, LOGGER_FACTORY) {
            Value = new MyParam()
        };
        // await setting.Read(Token);

        var proxyDescriptor = new SettingDescriptor("test1", typeof(string));
        var proxy = new MyParamSettingProxy(setting, proxyDescriptor) { Value = "value" };

        await proxy.CommitChanges(Token);
        transport.Verify(e => e.ExecOnStream(It.IsAny<Func<IMyDeviceStream,Task>>(), Token));
    }
    
    
    [Fact]
    public async Task ShouldNotWriteValue() {
        var transport = new Mock<IMyTransport>();
        var client = new Mock<IClient<IMyDeviceStream>>();
        var stream = new Mock<IMyDeviceStream>();
        client.Setup(e => e.GetStream()).Returns(stream.Object);
        transport.Setup(e => e.Client).Returns(client.Object);

        var setting = new MyParamSetting(transport.Object, SETTING_DESCRIPTORS, LOGGER_FACTORY) {
            Value = new MyParam()
        };
        // await setting.Read(Token);

        var proxyDescriptor = new SettingDescriptor("test1", typeof(string));
        var proxy = new MyParamSettingProxy(setting, proxyDescriptor) { Value = MyParamSettingProxy.FORBIDDEN_VALUES[0] };

        await proxy.CommitChanges(Token);
        
        Assert.True(proxy.IsDirty);
        Assert.False(proxy.IsValid);
        
        stream.Verify(e => e.Write(It.IsAny<byte[]>(), Token), Times.Never);
    }

    [Fact]
    public void ShouldCheckReadOnly() {
        var transport = new Mock<IMyTransport>();
        var setting = new MyParamSetting(new SettingDefinition<MyParam> {
            Reader = new MyParamReader(transport.Object, LOGGER_FACTORY),
            Descriptor = SETTING_DESCRIPTORS.MyParamSettingDescriptor,
        });

        var proxyDescriptor = new SettingDescriptor("test1", typeof(string));
        var proxy = new MyParamSettingProxy(setting, proxyDescriptor);
        
        Assert.True(proxy.ReadOnly);
    }
    
    
    [Fact]
    public async Task ShouldCatchExceptionWhileWrite() {
        var transport = new Mock<IMyTransport>();
        var ex = new Exception();


        var setting = new MyParamSetting(transport.Object, SETTING_DESCRIPTORS, LOGGER_FACTORY) {
            Value = new MyParam()
        };
        // await setting.Read(Token);

        transport.Setup(e => e.ExecOnStream(It.IsAny<Func<IMyDeviceStream,Task>>(), Token)).Throws(ex);

        var proxyDescriptor = new SettingDescriptor("test", typeof(string));
        var proxy = new MyParamSettingProxy(setting, proxyDescriptor) {
            Value = "test"
        };

        await proxy.CommitChanges(Token);
        
        Assert.Equal(ex, proxy.Exception);
        Assert.True(proxy.IsError);
    }
    
    
    [Fact]
    public async Task ShouldCatchExceptionWhileRead() {
        var transport = new Mock<IMyTransport>();
        var ex = new Exception();
        transport.Setup(e => e.ExecOnStream(It.IsAny<Func<IMyDeviceStream,Task>>(), Token)).Throws(ex);

        var setting = new MyParamSetting(transport.Object, SETTING_DESCRIPTORS, LOGGER_FACTORY);

        var proxyDescriptor = new SettingDescriptor("test1", typeof(string));
        var proxy = new MyParamSettingProxy(setting, proxyDescriptor);

        try {
            await proxy.Read(Token);
        } catch (Exception) { // ignored
        }


        Assert.Equal(ex, proxy.Exception);
        Assert.True(proxy.IsError);
    }
    
}