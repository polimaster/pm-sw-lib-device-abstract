using System;
using System.Threading.Tasks;
using Moq;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Tests.Impl.Device.Settings;

namespace Polimaster.Device.Abstract.Tests.Tests.Settings; 

public class MyParamSettingProxyTest : Mocks {

    [Fact]
    public void ShouldHaveDefaultBehaviour() {
        var reader = new Mock<IDataReader<MyParam?>>();
        var setting = new MyParamSetting(reader.Object);

        var proxy = new MyParamSettingProxy(setting);

        Assert.Null(proxy.Behaviour?.Name);
        Assert.Null(proxy.Behaviour?.GroupName);
        Assert.Equal(SettingAccessLevel.BASE, proxy.Behaviour?.AccessLevel);
    }

    [Fact]
    public void ShouldHaveValidBehaviour() {
        var reader = new Mock<IDataReader<MyParam?>>();
        var setting = new MyParamSetting(reader.Object);
        var myParamBehaviour = new SettingBehaviourBase {
            AccessLevel = SettingAccessLevel.EXTENDED,
            Name = "MyParamSettingProxyTest",
            GroupName = "MyParamSettingGroup"
        };

        var proxy = new MyParamSettingProxy(setting, myParamBehaviour);

        Assert.Equal(myParamBehaviour.Name, proxy.Behaviour?.Name);
        Assert.Equal(myParamBehaviour.GroupName, proxy.Behaviour?.GroupName);
        Assert.Equal(myParamBehaviour.AccessLevel, proxy.Behaviour?.AccessLevel);
    }


    [Fact]
    public async Task ShouldSetProperty() {
        var reader = new Mock<IDataReader<MyParam?>>();
        
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
        var reader = new Mock<IDataReader<MyParam?>>();

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
        
        Assert.False(proxy.IsDirty);
        Assert.False(proxy.IsValid);
    }
    
    [Fact]
    public async Task ShouldRead() {
        var setting = new Mock<IDeviceSetting<MyParam?>>();
        var proxy = new MyParamSettingProxy(setting.Object);

        await proxy.Read(Token);
        setting.Verify(e => e.Read(Token));
    }
    
    [Fact]
    public async Task ShouldWrite() {
        var reader = new Mock<IDataReader<MyParam?>>();
        var writer = new Mock<IDataWriter<MyParam?>>();

        var setting = new MyParamSetting(reader.Object, writer.Object);
        await setting.Read(Token);
        var proxy = new MyParamSettingProxy(setting) { Value = "value" };

        await proxy.CommitChanges(Token);
        writer.Verify(e => e.Write(It.IsAny<MyParam>(), Token));
    }
    
    
    [Fact]
    public async Task ShouldNotWriteValue() {
        var reader = new Mock<IDataReader<MyParam?>>();
        var writer = new Mock<IDataWriter<MyParam?>>();
        
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
        var reader = new Mock<IDataReader<MyParam?>>();
        
        var setting = new MyParamSetting(reader.Object);
        var proxy = new MyParamSettingProxy(setting);
        
        Assert.True(proxy.ReadOnly);
    }
    
    
    [Fact]
    public async Task ShouldCatchExceptionWhileWrite() {
        var reader = new Mock<IDataReader<MyParam?>>();
        var writer = new Mock<IDataWriter<MyParam?>>();
        
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
        var reader = new Mock<IDataReader<MyParam?>>();
        var ex = new Exception();
        reader.Setup(e => e.Read(Token)).ThrowsAsync(ex, TimeSpan.FromSeconds(2));
        
        var setting = new MyParamSetting(reader.Object);
        var proxy = new MyParamSettingProxy(setting);

        await proxy.Read(Token);
        Assert.Equal(ex, proxy.Exception);
        Assert.True(proxy.IsError);
    }
    
}