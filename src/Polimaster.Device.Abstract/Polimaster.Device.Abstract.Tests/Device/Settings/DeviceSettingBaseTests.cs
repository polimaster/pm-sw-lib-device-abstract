using System;
using System.Linq;
using System.Threading;
using Moq;
using Polimaster.Device.Abstract.Tests.Device.Commands;

namespace Polimaster.Device.Abstract.Tests.Device.Settings; 

public class DeviceSettingBaseTests : Mocks {

    [Fact]
    public async void ShouldSetDirtyAndDoNotCallWriteCommandSendUntilValueChanged() {
        var commandMock = CommandMock;
        var setting = new MyDeviceSetting {
            WriteCommand = commandMock.Object
        };

        await setting.CommitChanges(CancellationToken.None);
        Assert.False(setting.IsDirty);
        commandMock.Verify(x => x.Send(It.IsAny<CancellationToken>()), Times.Never);

        setting.Value = new MyParam { Value = "COMMAND_VALUE" };
        Assert.True(setting.IsDirty);
        
        await setting.CommitChanges(CancellationToken.None);
        commandMock.Verify(x => x.Send(It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async void ShouldCatchReadCommandException() {
        var commandMock = CommandMock;
        var ex = new Exception();
        commandMock.Setup(x => x.Send(It.IsAny<CancellationToken>())).ThrowsAsync(ex);
        
        var setting = new MyDeviceSetting {
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
        
        var setting = new MyDeviceSetting {
            WriteCommand = commandMock.Object,
            Value = new MyParam { Value = "COMMAND_VALUE" }
        };
        
        await setting.CommitChanges(CancellationToken.None);
        
        Assert.True(setting.IsError);
        Assert.Equal(ex, setting.Exception);
    }


    [Fact]
    public async void ShouldRead() {
        var commandValue = new MyParam { Value = "COMMAND_VALUE" };

        var readCommandMock = CommandMock;
        readCommandMock.Setup(x => x.Value).Returns(commandValue);

        var setting = new MyDeviceSetting {
            ReadCommand = readCommandMock.Object
        };

        await setting.Read(CancellationToken.None);
        Assert.Equal(commandValue, setting.Value); // check setting value initialization 
        readCommandMock.Verify(x => x.Send(It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async void ShouldCatchChangedCommandValue() {
        const string commandValue = "READ-VALUE";
        var writerMock = DeviceStreamMock;
        writerMock.Setup(e => e.ReadLineAsync(It.IsAny<CancellationToken>())).ReturnsAsync(commandValue);
        
        var transportMock = TransportMock;
        transportMock.Setup(x => x.Open()).ReturnsAsync(writerMock.Object);

        var readCommand = new MyReadCommand {
            Device = new MyDevice { Transport = transportMock.Object }
        };

        var setting = new MyDeviceSetting {
            ReadCommand = readCommand
        };
        
        await setting.Read(CancellationToken.None);
        
        Assert.Equal(commandValue, setting.Value?.Value); // check setting value changed on command ValueChanged event
        
    }
    

    [Fact]
    public async void ShouldCommit() {
        var value = new MyParam { Value = "COMMAND_VALUE" };
        var writeCommandMock = CommandMock;

        var setting = new MyDeviceSetting {
            WriteCommand = writeCommandMock.Object,
            Value = value
        };

        await setting.CommitChanges(CancellationToken.None);
        
        writeCommandMock.Verify(x => x.Send(It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async void ShouldCheckValidation() {
        var value = new MyParam { Value = "COMMAND_VALUE" };
        var writeCommandMock = CommandMock;

        var setting = new MyDeviceSettingValidatable {
            WriteCommand = writeCommandMock.Object,
            Value = value
        };
        
        Assert.False(setting.IsValid);
        Assert.True(setting.ValidationErrors?.Any());
        
        await setting.CommitChanges(CancellationToken.None);
        
        writeCommandMock.Verify(x => x.Send(It.IsAny<CancellationToken>()), Times.Never);
    }
}