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
    //     var reader = new Mock<IDataReader<MyParam>>();
    //     var setting = new MyParamSetting(reader.Object);
    //
    //     var proxy = new MyParamSettingProxy(setting);
    //
    //     Assert.Null(proxy.Descriptor?.Name);
    //     Assert.Null(proxy.Descriptor?.GroupName);
    //     Assert.Equal(SettingAccessLevel.BASE, proxy.Descriptor?.AccessLevel);
    // }

    [Fact]
    public void ShouldHaveValidDescriptor() {
        var reader = new Mock<IDataReader<MyParam>>();
        var setting = new MyParamSetting(reader.Object);
        var settingDescriptor = new SettingDescriptorBase("MyParamSettingProxyTest", SettingAccessLevel.EXTENDED, "MyParamSettingGroup");

        var proxy = new MyParamSettingProxy(setting, settingDescriptor);

        Assert.Equal(settingDescriptor.Name, proxy.Descriptor?.Name);
        Assert.Equal(settingDescriptor.GroupName, proxy.Descriptor?.GroupName);
        Assert.Equal(settingDescriptor.AccessLevel, proxy.Descriptor?.AccessLevel);
    }


    [Fact]
    public async Task ShouldSetProperty() {
        var reader = new Mock<IDataReader<MyParam>>();
        reader.Setup(e => e.Read(Token)).ReturnsAsync(new MyParam());

        var setting = new MyParamSetting(reader.Object);
        await setting.Read(Token);

        var proxy = new MyParamSettingProxy(setting) {
            Value = "test"
        };

        Assert.True(proxy.IsDirty);
        Assert.Null(proxy.Exception);
    }
    
    [Fact]
    public async Task ShouldValidateValue() {
        var reader = new Mock<IDataReader<MyParam>>();

        var setting = new MyParamSetting(reader.Object);
        await setting.Read(Token);
        
        var proxy = new MyParamSettingProxy(setting) {
            Value = MyParamSettingProxy.FORBIDDEN_VALUES[0]
        };
        
        Assert.True(proxy.IsDirty);
        Assert.False(proxy.IsValid);
        
        proxy = new MyParamSettingProxy(setting) {
            Value = null
        };
        
        Assert.True(proxy.IsDirty);
        Assert.False(proxy.IsValid);
    }
    
    [Fact]
    public async Task ShouldRead() {
        var setting = new Mock<IDeviceSetting<MyParam>>();
        var proxy = new MyParamSettingProxy(setting.Object);

        await proxy.Read(Token);
        setting.Verify(e => e.Read(Token));
    }
    
    [Fact]
    public async Task ShouldWrite() {
        var reader = new Mock<IDataReader<MyParam>>();
        reader.Setup(e => e.Read(Token)).ReturnsAsync(new MyParam());
        var writer = new Mock<IDataWriter<MyParam>>();

        var setting = new MyParamSetting(reader.Object, writer.Object);
        await setting.Read(Token);
        var proxy = new MyParamSettingProxy(setting) { Value = "value" };

        await proxy.CommitChanges(Token);
        writer.Verify(e => e.Write(It.IsAny<MyParam>(), Token));
    }
    
    
    [Fact]
    public async Task ShouldNotWriteValue() {
        var reader = new Mock<IDataReader<MyParam>>();
        var writer = new Mock<IDataWriter<MyParam>>();
        
        var setting = new MyParamSetting(reader.Object, writer.Object);
        await setting.Read(Token);
        var proxy = new MyParamSettingProxy(setting) { Value = MyParamSettingProxy.FORBIDDEN_VALUES[0] };

        await proxy.CommitChanges(Token);
        
        Assert.True(proxy.IsDirty);
        Assert.False(proxy.IsValid);
        
        writer.Verify(e => e.Write(It.IsAny<MyParam>(), Token), Times.Never);
    }

    [Fact]
    public void ShouldCheckReadOnly() {
        var reader = new Mock<IDataReader<MyParam>>();
        
        var setting = new MyParamSetting(reader.Object);
        var proxy = new MyParamSettingProxy(setting);
        
        Assert.True(proxy.ReadOnly);
    }
    
    
    [Fact]
    public async Task ShouldCatchExceptionWhileWrite() {
        var reader = new Mock<IDataReader<MyParam>>();
        reader.Setup(e => e.Read(Token)).ReturnsAsync(new MyParam {Value = "value from device"});
        var writer = new Mock<IDataWriter<MyParam>>();
        
        var setting = new MyParamSetting(reader.Object, writer.Object);
        await setting.Read(Token);
        var proxy = new MyParamSettingProxy(setting) {
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
        
        var setting = new MyParamSetting(reader.Object);
        var proxy = new MyParamSettingProxy(setting);

        try {
            await proxy.Read(Token);
        } catch (Exception) { // ignored
        }


        Assert.Equal(ex, proxy.Exception);
        Assert.True(proxy.IsError);
    }
    
}