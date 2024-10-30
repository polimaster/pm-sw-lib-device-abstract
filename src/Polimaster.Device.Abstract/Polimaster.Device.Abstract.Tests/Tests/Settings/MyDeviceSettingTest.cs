using System;
using System.Threading.Tasks;
using Moq;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Tests.Impl.Device.Settings;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Tests.Settings;

public class MyDeviceSettingTest : Mocks {

    [Fact]
    public void ShouldHaveDefaultBehaviour() {
        var reader = new Mock<IDataReader<MyParam?>>();
        var writer = new Mock<IDataWriter<MyParam?>>();

        var p = new MyParam { Value = "test" };
        var setting = new MyParamSetting(reader.Object, writer.Object) {
            Value = p
        };

        Assert.Null(setting.Behaviour?.GroupName);
        Assert.Equal(SettingAccessLevel.BASE, setting.Behaviour?.AccessLevel);
    }

    [Fact]
    public void ShouldHaveValidBehaviour() {
        var reader = new Mock<IDataReader<MyParam?>>();
        var writer = new Mock<IDataWriter<MyParam?>>();
        var myParamBehaviour = new SettingBehaviourBase(SettingAccessLevel.BASE, "MyParamSettingGroup");

        var p = new MyParam { Value = "test" };
        var setting = new MyParamSetting(reader.Object, writer.Object, myParamBehaviour) {
            Value = p
        };

        Assert.Equal(myParamBehaviour.GroupName, setting.Behaviour?.GroupName);
        Assert.Equal(myParamBehaviour.AccessLevel, setting.Behaviour?.AccessLevel);
    }

    
    [Fact]
    public void ShouldSetProperty() {
        var reader = new Mock<IDataReader<MyParam?>>();

        var p = new MyParam();
        var setting = new MyParamSetting(reader.Object) {
            Value = p
        };

        Assert.True(setting.IsDirty);
        Assert.Null(setting.Exception);
    }
    
    [Fact]
    public void ShouldValidateValue() {
        var reader = new Mock<IDataReader<MyParam?>>();

        var setting = new MyParamSetting(reader.Object) {
            Value = null
        };
        Assert.True(setting.IsDirty);
        Assert.False(setting.IsValid);

        setting = new MyParamSetting(reader.Object) {
            Value = new MyParam { Value = "Very long string that does not pass validation" }
        };
        Assert.True(setting.IsDirty);
        Assert.False(setting.IsValid);

        setting = new MyParamSetting(reader.Object) {
            Value = new MyParam { Value = "Valid" }
        };
        Assert.True(setting.IsDirty);
        Assert.True(setting.IsValid);
    }

    [Fact]
    public async Task ShouldRead() {
        var transport = new Mock<ITransport>();
        var reader = new Mock<IDataReader<MyParam?>>();
        var p = new MyParam();
        transport.Setup(e => e.Read(reader.Object, Token)).Returns(Task.FromResult(p)!);
        
        var setting = new MyParamSetting(reader.Object);

        await setting.Read(transport.Object, Token);
        
        transport.Verify(e => e.Read(reader.Object, Token));
        Assert.Equal(p, setting.Value);
    }

    [Fact]
    public async Task ShouldWrite() {
        var transport = new Mock<ITransport>();
        var reader = new Mock<IDataReader<MyParam?>>();
        var writer = new Mock<IDataWriter<MyParam?>>();

        var p = new MyParam { Value = "test" };
        var setting = new MyParamSetting(reader.Object, writer.Object) {
            Value = p
        };

        await setting.CommitChanges(transport.Object, Token);
        
        transport.Verify(e => e.Write(writer.Object, p, Token));
    }

    [Fact]
    public async Task ShouldNotWriteValue() {
        var transport = new Mock<ITransport>();
        var reader = new Mock<IDataReader<MyParam?>>();
        var writer = new Mock<IDataWriter<MyParam?>>();

        var p = new MyParam { Value = "ve__________________________ery long string" };
        var setting = new MyParamSetting(reader.Object, writer.Object) {
            Value = p
        };

        await setting.CommitChanges(transport.Object, Token);
        
        Assert.True(setting.IsError);
        Assert.True(setting.IsDirty);
        Assert.NotNull(setting.Exception);
        
        transport.Verify(e => e.Write(writer.Object, p, Token), Times.Never);
    }

    [Fact]
    public async Task ShouldNotWriteReadOnly() {
        var transport = new Mock<ITransport>();
        var reader = new Mock<IDataReader<MyParam?>>();

        var p = new MyParam { Value = "test" };
        var setting = new MyParamSetting(reader.Object) {
            Value = p
        };

        await setting.CommitChanges(transport.Object, Token);
        
        transport.Verify(e => e.Write(It.IsAny<IDataWriter<MyParam>>(), p, Token), Times.Never);
    }

    [Fact]
    public async Task ShouldCatchExceptionWhileWrite() {
        var transport = new Mock<ITransport>();
        var reader = new Mock<IDataReader<MyParam?>>();
        var writer = new Mock<IDataWriter<MyParam?>>();
        
        var p = new MyParam { Value = "test" };
        var setting = new MyParamSetting(reader.Object, writer.Object) {
            Value = p
        };

        var ex = new Exception();
        transport.Setup(e => e.Write(writer.Object, p, Token)).ThrowsAsync(ex);
        
        await setting.CommitChanges(transport.Object, Token);
        
        Assert.Equal(ex, setting.Exception);
        Assert.True(setting.IsError);
    }
    

    [Fact]
    public async Task ShouldCatchExceptionWhileRead() {
        var transport = new Mock<ITransport>();
        var reader = new Mock<IDataReader<MyParam?>>();
        var ex = new Exception();
        
        transport.Setup(e => e.Read(reader.Object, Token)).ThrowsAsync(ex, TimeSpan.FromSeconds(2));
        
        var setting = new MyParamSetting(reader.Object);

        await setting.Read(transport.Object, Token);
        Assert.Equal(ex, setting.Exception);
        Assert.Null(setting.Value);
        Assert.True(setting.IsError);
    }
    
}