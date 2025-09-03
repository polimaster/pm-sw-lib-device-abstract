using System;
using System.Threading.Tasks;
using Moq;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Tests.Impl.Settings;
using Polimaster.Device.Abstract.Tests.Impl.Transport;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Tests.Settings;

public class DeviceSettingTest : Mocks {

    // [Fact]
    // public void ShouldHaveDefaultDescriptor() {
    //     var reader = new Mock<IDataReader<MyParam>>();
    //     var writer = new Mock<IDataWriter<MyParam>>();
    //
    //     var p = new MyParam { Value = "test" };
    //     var setting = new MyParamSetting(reader.Object, writer.Object) {
    //         Value = p
    //     };
    //
    //     Assert.Null(setting.Descriptor?.Name);
    //     Assert.Null(setting.Descriptor?.GroupName);
    //     Assert.Equal(SettingAccessLevel.BASE, setting.Descriptor?.AccessLevel);
    // }

    [Fact]
    public void ShouldHaveValidDescriptor() {
        var settingDescriptor = SETTING_DESCRIPTORS.MyParamSettingDescriptor;
        var transport = new Mock<IMyTransport>();

        var p = new MyParam { Value = "test" };
        var setting = new MyParamSetting(transport.Object, SETTING_DESCRIPTORS, LOGGER_FACTORY) {
            Value = p
        };

        Assert.Equal(settingDescriptor.Name, setting.Descriptor.Name);
        Assert.Equal(settingDescriptor.GroupName, setting.Descriptor.GroupName);
        Assert.Equal(settingDescriptor.AccessLevel, setting.Descriptor.AccessLevel);
    }


    [Fact]
    public void ShouldSetProperty() {
        var transport = new Mock<IMyTransport>();

        var p = new MyParam();
        var setting = new MyParamSetting(transport.Object, SETTING_DESCRIPTORS, LOGGER_FACTORY) {
            Value = p
        };

        Assert.True(setting.IsDirty);
        Assert.Null(setting.Exception);
    }

    [Fact]
    public void ShouldValidateValue() {
        var transport = new Mock<IMyTransport>();

        var setting = new MyParamSetting(transport.Object, SETTING_DESCRIPTORS, LOGGER_FACTORY) {
            Value = null
        };
        Assert.True(setting.IsDirty);
        Assert.False(setting.IsValid);

        setting = new MyParamSetting(transport.Object, SETTING_DESCRIPTORS, LOGGER_FACTORY) {
            Value = new MyParam { Value = "Very long string that does not pass validation" }
        };
        Assert.True(setting.IsDirty);
        Assert.False(setting.IsValid);

        setting = new MyParamSetting(transport.Object, SETTING_DESCRIPTORS, LOGGER_FACTORY) {
            Value = new MyParam { Value = "Valid" }
        };
        Assert.True(setting.IsDirty);
        Assert.True(setting.IsValid);
    }

    [Fact]
    public async Task ShouldRead() {
        var reader = new Mock<IDataReader<MyParam>>();
        var p = new MyParam();
        reader.Setup(e => e.Read(Token)).Returns(Task.FromResult(p));
        var setting = new MyParamSetting(new SettingDefinition<MyParam> {
            Reader = reader.Object,
            Descriptor = SETTING_DESCRIPTORS.MyParamSettingDescriptor,
        });

        await setting.Read(Token);

        reader.Verify(e => e.Read(Token));
        Assert.Equal(p, setting.Value);
    }

    [Fact]
    public async Task ShouldWrite() {

        var reader = new Mock<IDataReader<MyParam>>();
        var writer = new Mock<IDataWriter<MyParam>>();

        var p = new MyParam { Value = "test" };
        var setting = new MyParamSetting(new SettingDefinition<MyParam> {
            Reader = reader.Object,
            Writer = writer.Object,
            Descriptor = SETTING_DESCRIPTORS.MyParamSettingDescriptor,
        }) {
            Value = p
        };

        await setting.CommitChanges(Token);

        writer.Verify(e => e.Write(p, Token));
    }

    [Fact]
    public async Task ShouldNotWriteValue() {

        var reader = new Mock<IDataReader<MyParam>>();
        var writer = new Mock<IDataWriter<MyParam>>();

        var p = new MyParam { Value = "ve__________________________ery long string" };
        var setting = new MyParamSetting(new SettingDefinition<MyParam> {
            Reader = reader.Object,
            Writer = writer.Object,
            Descriptor = SETTING_DESCRIPTORS.MyParamSettingDescriptor,
        }) {
            Value = p
        };

        await setting.CommitChanges(Token);

        Assert.True(setting.IsError);
        Assert.True(setting.IsDirty);
        Assert.NotNull(setting.Exception);

        writer.Verify(e => e.Write(p, Token), Times.Never);
    }

    [Fact]
    public async Task ShouldNotWriteReadOnly() {
        var transport = new Mock<IMyTransport>();
        var client = new Mock<IClient<IMyDeviceStream>>();
        var stream = new Mock<IMyDeviceStream>();
        client.Setup(e => e.GetStream()).Returns(stream.Object);
        transport.Setup(e => e.Client).Returns(client.Object);

        var p = new MyParam { Value = "test" };
        var setting = new MyParamSetting(transport.Object, SETTING_DESCRIPTORS, LOGGER_FACTORY) {
            Value = p
        };

        await setting.CommitChanges(Token);
        stream.Verify(e => e.Write(It.IsAny<byte[]>(), Token), Times.Never);
        // transport.Verify(e => e.WriteAsync(It.IsAny<byte[]>(), Token), Times.Never);
    }

    [Fact]
    public async Task ShouldCatchExceptionWhileWrite() {

        var reader = new Mock<IDataReader<MyParam>>();
        var writer = new Mock<IDataWriter<MyParam>>();

        var p = new MyParam { Value = "test" };
        var setting = new MyParamSetting(new SettingDefinition<MyParam> {
            Reader = reader.Object,
            Writer = writer.Object,
            Descriptor = SETTING_DESCRIPTORS.MyParamSettingDescriptor,
        }) {
            Value = p
        };

        var ex = new Exception();
        writer.Setup(e => e.Write(It.IsAny<MyParam>(), Token)).ThrowsAsync(ex);

        await setting.CommitChanges(Token);

        Assert.Equal(ex, setting.Exception);
        Assert.True(setting.IsError);
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

        await setting.Read(Token);
        Assert.Equal(ex, setting.Exception);
        Assert.Null(setting.Value);
        Assert.True(setting.IsError);
    }

}