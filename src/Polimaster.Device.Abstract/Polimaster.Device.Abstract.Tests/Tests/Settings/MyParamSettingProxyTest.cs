using System;
using System.Threading.Tasks;
using Moq;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Tests.Impl.Device.Settings;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Tests.Settings; 

public class MyParamSettingProxyTest : Mocks {

    [Fact]
    public void ShouldHaveValidBehaviour() {
        var reader = new Mock<IDataReader<MyParam?>>();
        var setting = new MyParamSetting(reader.Object);
        var myParamBehaviour = new SettingBehaviourBase(SettingAccessLevel.EXTENDED, "MyParamSettingGroup");

        var proxy = new MyParamSettingProxy(setting, myParamBehaviour);

        Assert.Equal(myParamBehaviour.GroupName, proxy.Behaviour?.GroupName);
        Assert.Equal(myParamBehaviour.AccessLevel, proxy.Behaviour?.AccessLevel);
    }


    [Fact]
    public async void ShouldSetProperty() {
        var reader = new Mock<IDataReader<MyParam?>>();
        var transport = new Mock<ITransport>();
        
        var setting = new MyParamSetting(reader.Object);
        await setting.Read(transport.Object, Token);

        var proxy = new MyParamSettingProxy(setting) {
            Value = "test"
        };

        Assert.True(proxy.IsDirty);
        Assert.Null(proxy.Exception);
    }
    
    [Fact]
    public async void ShouldValidateValue() {
        var reader = new Mock<IDataReader<MyParam?>>();
        var transport = new Mock<ITransport>();

        var setting = new MyParamSetting(reader.Object);
        await setting.Read(transport.Object, Token);
        
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
    public async void ShouldRead() {
        var transport = new Mock<ITransport>();
        var reader = new Mock<IDataReader<MyParam?>>();
        var p = new MyParam { Value = "123456" };
        transport.Setup(e => e.Read(reader.Object, Token)).Returns(Task.FromResult(p)!);
        
        var setting = new MyParamSetting(reader.Object);
        var proxy = new MyParamSettingProxy(setting);

        await proxy.Read(transport.Object, Token);
        
        transport.Verify(e => e.Read(reader.Object, Token));
        Assert.Equal(p.Value, proxy.Value);
    }
    
    [Fact]
    public async void ShouldWrite() {
        var transport = new Mock<ITransport>();
        var reader = new Mock<IDataReader<MyParam?>>();
        var writer = new Mock<IDataWriter<MyParam?>>();

        var p = new MyParam { Value = "test" };
        var setting = new MyParamSetting(reader.Object, writer.Object) { Value = p };
        await setting.Read(transport.Object, Token);
        var proxy = new MyParamSettingProxy(setting) { Value = "value" };

        await proxy.CommitChanges(transport.Object, Token);
        
        transport.Verify(e => e.Write(writer.Object, setting.Value, Token));
    }
    
    
    [Fact]
    public async void ShouldNotWriteValue() {
        var transport = new Mock<ITransport>();
        var reader = new Mock<IDataReader<MyParam?>>();
        var writer = new Mock<IDataWriter<MyParam?>>();
        
        var setting = new MyParamSetting(reader.Object, writer.Object);
        await setting.Read(transport.Object, Token);
        var proxy = new MyParamSettingProxy(setting) { Value = MyParamSettingProxy.FORBIDDEN_VALUES[0] };

        await proxy.CommitChanges(transport.Object, Token);
        
        Assert.True(proxy.IsDirty);
        Assert.False(proxy.IsValid);
        
        transport.Verify(e => e.Write(writer.Object, It.IsAny<MyParam>(), Token), Times.Never);
    }

    [Fact]
    public void ShouldCheckReadOnly() {
        var reader = new Mock<IDataReader<MyParam?>>();
        
        var setting = new MyParamSetting(reader.Object);
        var proxy = new MyParamSettingProxy(setting);
        
        Assert.True(proxy.ReadOnly);
    }
    
    
    [Fact]
    public async void ShouldCatchExceptionWhileWrite() {
        var transport = new Mock<ITransport>();
        var reader = new Mock<IDataReader<MyParam?>>();
        var writer = new Mock<IDataWriter<MyParam?>>();
        
        var setting = new MyParamSetting(reader.Object, writer.Object);
        await setting.Read(transport.Object, Token);
        var proxy = new MyParamSettingProxy(setting) {
            Value = "test"
        };

        var ex = new Exception();
        transport.Setup(e => e.Write(writer.Object, It.IsAny<MyParam>(), Token)).ThrowsAsync(ex);

        
        await proxy.CommitChanges(transport.Object, Token);
        
        Assert.Equal(ex, proxy.Exception);
        Assert.True(proxy.IsError);
    }
    
    
    [Fact]
    public async void ShouldCatchExceptionWhileRead() {
        var transport = new Mock<ITransport>();
        var reader = new Mock<IDataReader<MyParam?>>();
        var ex = new Exception();
        transport.Setup(e => e.Read(reader.Object, Token)).ThrowsAsync(ex, TimeSpan.FromSeconds(2));
        
        var setting = new MyParamSetting(reader.Object);
        var proxy = new MyParamSettingProxy(setting);

        await proxy.Read(transport.Object, Token);
        Assert.Equal(ex, proxy.Exception);
        Assert.True(proxy.IsError);
    }
    
}