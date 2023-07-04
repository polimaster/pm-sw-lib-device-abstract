using System;
using System.IO;
using System.Threading;
using Moq;
using Polimaster.Device.Abstract.Device.Settings;

namespace Polimaster.Device.Abstract.Tests.Device.Settings; 

public class DeviceSettingBaseTests : Mocks {

    [Fact]
    public async void ShouldSetDirtyAndDoNotCallWriteCommandSendUntilValueChanged() {
        var commandMock = CommandMock;
        var setting = new DeviceSettingBase<string> {
            WriteCommand = commandMock.Object
        };

        await setting.CommitChanges(CancellationToken.None);
        Assert.False(setting.IsDirty);
        commandMock.Verify(x => x.Send(It.IsAny<CancellationToken>()), Times.Never);

        setting.Value = "value";
        Assert.True(setting.IsDirty);
        
        await setting.CommitChanges(CancellationToken.None);
        commandMock.Verify(x => x.Send(It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async void ShouldCatchReadCommandException() {
        var commandMock = CommandMock;
        var ex = new Exception();
        commandMock.Setup(x => x.Send(It.IsAny<CancellationToken>())).ThrowsAsync(ex);
        
        var setting = new DeviceSettingBase<string> {
            ReadCommand = commandMock.Object
        };
        
        await setting.Read(CancellationToken.None);
        
        Assert.True(setting.IsError);
        Assert.Null(setting.Value);
        Assert.Equal(ex, setting.Exception);
    }

    [Fact]
    public async void ShouldCatchWriteCommandException() {
        var commandMock = CommandMock;
        var ex = new Exception();
        commandMock.Setup(x => x.Send(It.IsAny<CancellationToken>())).ThrowsAsync(ex);
        
        var setting = new DeviceSettingBase<string> {
            WriteCommand = commandMock.Object,
            Value = "value"
        };
        
        await setting.CommitChanges(CancellationToken.None);
        
        Assert.True(setting.IsError);
        Assert.Equal(ex, setting.Exception);
    }


    [Fact]
    public async void ShouldRead() {
        const string commandValue = "COMMAND_VALUE";

        var readCommandMock = CommandMock;
        readCommandMock.Setup(x => x.Value).Returns(commandValue);

        var setting = new DeviceSettingBase<string> {
            ReadCommand = readCommandMock.Object
        };

        await setting.Read(CancellationToken.None);
        Assert.Equal(commandValue, setting.Value); // check setting value initialization 
        readCommandMock.Verify(x => x.Send(It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async void ShouldCatchChangedCommandValue() {
        const string commandValue = "COMMAND_VALUE";

        var transportMock = TransportMock;
        transportMock.Setup(x => x.Open()).ReturnsAsync(StreamMock.Object);
        transportMock.Setup(x => x.Read(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(commandValue);
        var readCommand = new MyResultCommand {
            Transport = transportMock.Object
        };

        var setting = new DeviceSettingBase<string> {
            ReadCommand = readCommand
        };
        
        await setting.Read(CancellationToken.None);
        
        Assert.Equal(commandValue, setting.Value); // check setting value changed on command ValueChanged event
        
    }
    

    [Fact]
    public async void ShouldCommit() {
        const string value = "SETTING_VALUE";
        var writeCommandMock = CommandMock;

        var setting = new DeviceSettingBase<string> {
            WriteCommand = writeCommandMock.Object,
            Value = value
        };

        await setting.CommitChanges(CancellationToken.None);
        
        writeCommandMock.Verify(x => x.Send(It.IsAny<CancellationToken>()));
    }
    
}