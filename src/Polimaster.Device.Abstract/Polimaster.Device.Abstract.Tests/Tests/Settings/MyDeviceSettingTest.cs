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

        var p = new MyParam { Value = "test" };
        var setting = new MyParamSetting(transport.Object, SETTING_DESCRIPTORS, LOGGER_FACTORY) {
            Value = p
        };

        Assert.True(setting.IsDirty);
        Assert.True(setting.HasValue);
        Assert.False(setting.IsSynchronized);
        Assert.False(setting.IsError);
        Assert.Empty(setting.ValidationResults);
        Assert.Null(setting.Exception);
        Assert.NotNull(setting.UntypedValue);
        Assert.NotNull(setting.Value);
    }

    [Fact]
    public void ShouldRaisePropertyChanged() {
        var transport = new Mock<IMyTransport>();
        var p = new MyParam { Value = "test" };
        var setting = new MyParamSetting(transport.Object, SETTING_DESCRIPTORS, LOGGER_FACTORY);

        var valuePropertyRaised = false;
        var isDirtyPropertyRaised = false;
        var untypedValuePropertyRaised = false;
        var hasValuePropertyRaised = false;
        var isValidPropertyRaised = false;
        var validationErrorsPropertyRaised = false;

        setting.PropertyChanged += (_, args) => {
            switch (args.PropertyName) {
                case nameof(setting.Value):
                    valuePropertyRaised = true;
                    break;
                case nameof(setting.UntypedValue):
                    untypedValuePropertyRaised = true;
                    break;
                case nameof(setting.IsDirty):
                    isDirtyPropertyRaised = true;
                    break;
                case nameof(setting.HasValue):
                    hasValuePropertyRaised = true;
                    break;
                case nameof(setting.IsValid):
                    isValidPropertyRaised = true;
                    break;
                case nameof(setting.ValidationResults):
                    validationErrorsPropertyRaised = true;
                    break;
            }
        };

        setting.Value = p;

        Assert.True(valuePropertyRaised);
        Assert.True(untypedValuePropertyRaised);
        Assert.True(isDirtyPropertyRaised);
        Assert.True(hasValuePropertyRaised);
        Assert.True(isValidPropertyRaised);
        Assert.True(validationErrorsPropertyRaised);
    }


    [Fact]
    public void ShouldValidateValue() {
        var transport = new Mock<IMyTransport>();

        var setting = new MyParamSetting(transport.Object, SETTING_DESCRIPTORS, LOGGER_FACTORY);

        var isValidPropertyRaised = false;
        var validationErrorsPropertyRaised = false;
        setting.PropertyChanged += (_, args) => {
            switch (args.PropertyName) {
                case nameof(setting.IsValid):
                    isValidPropertyRaised = true;
                    break;
                case nameof(setting.ValidationResults):
                    validationErrorsPropertyRaised = true;
                    break;
            }
        };

        setting.Value = null;

        Assert.True(setting.IsDirty);
        Assert.False(setting.IsValid);
        Assert.True(isValidPropertyRaised);
        Assert.True(validationErrorsPropertyRaised);


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

        var valuePropertyRaised = false;
        var untypedValuePropertyRaised = false;
        var isSynchronizedPropertyRaised = false;
        var hasValuePropertyRaised = false;

        setting.PropertyChanged += (_, args) => {
            switch (args.PropertyName) {
                case nameof(setting.Value):
                    valuePropertyRaised = true;
                    break;
                case nameof(setting.UntypedValue):
                    untypedValuePropertyRaised = true;
                    break;
                case nameof(setting.IsSynchronized):
                    isSynchronizedPropertyRaised = true;
                    break;
                case nameof(setting.HasValue):
                    hasValuePropertyRaised = true;
                    break;
            }
        };

        await setting.Read(Token);

        reader.Verify(e => e.Read(Token));
        Assert.Equal(p, setting.Value);

        Assert.True(valuePropertyRaised);
        Assert.True(untypedValuePropertyRaised);
        Assert.True(isSynchronizedPropertyRaised);
        Assert.True(hasValuePropertyRaised);
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

        var isSynchronizedPropertyRaised = false;
        var isDirtyPropertyRaised = false;
        setting.PropertyChanged += (_, args) => {
            switch (args.PropertyName) {
                case nameof(setting.IsSynchronized):
                    isSynchronizedPropertyRaised = true;
                    break;
                case nameof(setting.IsDirty):
                    isDirtyPropertyRaised = true;
                    break;
            }
        };


        await setting.CommitChanges(Token);

        writer.Verify(e => e.Write(p, Token));
        Assert.True(isSynchronizedPropertyRaised);
        Assert.True(isDirtyPropertyRaised);
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

        var isErrorPropertyRaised = false;
        var exceptionPropertyRaised = false;
        setting.PropertyChanged += (_, args) => {
            switch (args.PropertyName) {
                case nameof(setting.IsError):
                    isErrorPropertyRaised = true;
                    break;
                case nameof(setting.Exception):
                    exceptionPropertyRaised = true;
                    break;
            }
        };


        await setting.CommitChanges(Token);

        Assert.True(setting.IsError);
        Assert.True(setting.IsDirty);
        Assert.NotNull(setting.Exception);

        writer.Verify(e => e.Write(p, Token), Times.Never);
        Assert.True(isErrorPropertyRaised);
        Assert.True(exceptionPropertyRaised);
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

        var isErrorPropertyRaised = false;
        var exceptionPropertyRaised = false;
        setting.PropertyChanged += (_, args) => {
            switch (args.PropertyName) {
                case nameof(setting.Exception):
                    exceptionPropertyRaised = true;
                    break;
                case nameof(setting.IsError):
                    isErrorPropertyRaised = true;
                    break;
            }
        };


        var ex = new Exception();
        writer.Setup(e => e.Write(It.IsAny<MyParam>(), Token)).ThrowsAsync(ex);

        await setting.CommitChanges(Token);

        Assert.Equal(ex, setting.Exception);
        Assert.True(setting.IsError);
        Assert.True(isErrorPropertyRaised);
        Assert.True(exceptionPropertyRaised);

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

        var isErrorPropertyRaised = false;
        var exceptionPropertyRaised = false;
        setting.PropertyChanged += (_, args) => {
            switch (args.PropertyName) {
                case nameof(setting.Exception):
                    exceptionPropertyRaised = true;
                    break;
                case nameof(setting.IsError):
                    isErrorPropertyRaised = true;
                    break;
            }
        };

        await setting.Read(Token);
        Assert.Equal(ex, setting.Exception);
        Assert.Null(setting.Value);
        Assert.True(setting.IsError);

        Assert.True(isErrorPropertyRaised);
        Assert.True(exceptionPropertyRaised);
    }

}