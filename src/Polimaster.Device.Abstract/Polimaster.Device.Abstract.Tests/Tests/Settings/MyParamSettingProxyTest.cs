using System;
using System.Threading.Tasks;
using Moq;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Tests.Impl.Settings;

namespace Polimaster.Device.Abstract.Tests.Tests.Settings; 

public class ADeviceSettingProxyTest : Mocks {

    [Fact]
    public void ShouldHaveDefaultDescriptor() {
        var reader = new Mock<IDataReader<MyParam>>();
        var descriptor = new SettingDescriptor<MyParam>("test");
        var setting = new MyParamSetting(reader.Object, descriptor);

        var proxyDescriptor = new SettingDescriptor<string>("MyParamSettingProxyTest", SettingAccessLevel.EXTENDED, "MyParamSettingGroup");
        var proxy = new MyParamSettingProxy(setting, proxyDescriptor);

        Assert.NotNull(proxy.Descriptor.Name);
        Assert.NotNull(proxy.Descriptor.GroupName);
        Assert.Equal(SettingAccessLevel.EXTENDED, proxy.Descriptor.AccessLevel);
    }

    [Fact]
    public void ShouldHaveValidDescriptor() {
        var reader = new Mock<IDataReader<MyParam>>();
        var descriptor = new SettingDescriptor<MyParam>("test");
        var setting = new MyParamSetting(reader.Object, descriptor);

        var proxyDescriptor = new SettingDescriptor<string>("MyParamSettingProxyTest", SettingAccessLevel.EXTENDED, "MyParamSettingGroup");
        var proxy = new MyParamSettingProxy(setting, proxyDescriptor);

        Assert.Equal(proxyDescriptor.Name, proxy.Descriptor.Name);
        Assert.Equal(proxyDescriptor.GroupName, proxy.Descriptor.GroupName);
        Assert.Equal(proxyDescriptor.AccessLevel, proxy.Descriptor.AccessLevel);
    }


    [Fact]
    public async Task ShouldSetProperty() {
        var reader = new Mock<IDataReader<MyParam>>();
        reader.Setup(e => e.Read(Token)).ReturnsAsync(new MyParam());

        var descriptor = new SettingDescriptor<MyParam>("test");
        var setting = new MyParamSetting(reader.Object, descriptor);
        await setting.Read(Token);

        var proxyDescriptor = new SettingDescriptor<string>("test1");
        var proxy = new MyParamSettingProxy(setting, proxyDescriptor) {
            Value = "test"
        };

        Assert.True(proxy.IsDirty);
        Assert.Null(proxy.Exception);
    }
    
    [Fact]
    public async Task ShouldValidateValue() {
        var reader = new Mock<IDataReader<MyParam>>();

        var descriptor = new SettingDescriptor<MyParam>("test");
        var setting = new MyParamSetting(reader.Object, descriptor);
        await setting.Read(Token);

        var proxyDescriptor = new SettingDescriptor<string>("test1");
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
        var proxyDescriptor = new SettingDescriptor<string>("test1");
        var proxy = new MyParamSettingProxy(setting.Object, proxyDescriptor);

        await proxy.Read(Token);
        setting.Verify(e => e.Read(Token));
    }
    
    [Fact]
    public async Task ShouldWrite() {
        var reader = new Mock<IDataReader<MyParam>>();
        reader.Setup(e => e.Read(Token)).ReturnsAsync(new MyParam());
        var writer = new Mock<IDataWriter<MyParam>>();

        var descriptor = new SettingDescriptor<MyParam>("test1");
        var setting = new MyParamSetting(reader.Object, descriptor, writer.Object);
        await setting.Read(Token);

        var proxyDescriptor = new SettingDescriptor<string>("test1");
        var proxy = new MyParamSettingProxy(setting, proxyDescriptor) { Value = "value" };

        await proxy.CommitChanges(Token);
        writer.Verify(e => e.Write(It.IsAny<MyParam>(), Token));
    }
    
    
    [Fact]
    public async Task ShouldNotWriteValue() {
        var reader = new Mock<IDataReader<MyParam>>();
        var writer = new Mock<IDataWriter<MyParam>>();

        var descriptor = new SettingDescriptor<MyParam>("test");
        var setting = new MyParamSetting(reader.Object, descriptor, writer.Object);
        await setting.Read(Token);
        var proxyDescriptor = new SettingDescriptor<string>("test1");
        var proxy = new MyParamSettingProxy(setting, proxyDescriptor) { Value = MyParamSettingProxy.FORBIDDEN_VALUES[0] };

        await proxy.CommitChanges(Token);
        
        Assert.True(proxy.IsDirty);
        Assert.False(proxy.IsValid);
        
        writer.Verify(e => e.Write(It.IsAny<MyParam>(), Token), Times.Never);
    }

    [Fact]
    public void ShouldCheckReadOnly() {
        var reader = new Mock<IDataReader<MyParam>>();

        var descriptor = new SettingDescriptor<MyParam>("test");
        var setting = new MyParamSetting(reader.Object, descriptor);

        var proxyDescriptor = new SettingDescriptor<string>("test1");
        var proxy = new MyParamSettingProxy(setting, proxyDescriptor);
        
        Assert.True(proxy.ReadOnly);
    }
    
    
    [Fact]
    public async Task ShouldCatchExceptionWhileWrite() {
        var reader = new Mock<IDataReader<MyParam>>();
        reader.Setup(e => e.Read(Token)).ReturnsAsync(new MyParam {Value = "value from device"});
        var writer = new Mock<IDataWriter<MyParam>>();

        var descriptor = new SettingDescriptor<MyParam>("test");
        var setting = new MyParamSetting(reader.Object, descriptor, writer.Object);
        await setting.Read(Token);

        var proxyDescriptor = new SettingDescriptor<string>("test");
        var proxy = new MyParamSettingProxy(setting, proxyDescriptor) {
            Value = "test"
        };

        var ex = new Exception();
        writer.Setup(e => e.Write(It.IsAny<MyParam>(), Token)).ThrowsAsync(ex);

        await proxy.CommitChanges(Token);
        
        Assert.Equal(ex, proxy.Exception);
        Assert.True(proxy.IsError);
    }
    
    
    [Fact]
    public async Task ShouldCatchExceptionWhileRead() {
        var reader = new Mock<IDataReader<MyParam>>();
        var ex = new Exception();
        reader.Setup(e => e.Read(Token)).ThrowsAsync(ex);

        var descriptor = new SettingDescriptor<MyParam>("test");
        var setting = new MyParamSetting(reader.Object, descriptor);

        var proxyDescriptor = new SettingDescriptor<string>("test1");
        var proxy = new MyParamSettingProxy(setting, proxyDescriptor);

        try {
            await proxy.Read(Token);
        } catch (Exception) { // ignored
        }


        Assert.Equal(ex, proxy.Exception);
        Assert.True(proxy.IsError);
    }
    
}