using System;
using System.IO;
using System.Threading;
using Moq;

namespace Polimaster.Device.Abstract.Tests.Device.Commands; 

public class WriteCommandTests : Mocks {

    [Fact]
    public async void ShouldThrowExceptionIfTransportNUll() {

        var command = new MyWriteCommand();
        
        await Assert.ThrowsAsync<NullReferenceException>(() => command.Send());
    }

    [Fact]
    public async void ShouldCancelSend() {
        var transportMock = TransportMock;
        var token = new CancellationToken(true);
        transportMock.Setup( x => x.Open()).ReturnsAsync(StreamMock.Object);
        
        var command = new MyWriteCommand {
            Transport = transportMock.Object
        };

        await command.Send(token);
        
        transportMock.Verify(x => x.Write(It.IsAny<Stream>(), It.IsAny<string>(), token), Times.Never);
    }

    [Fact]
    public async void ShouldCallValueChanged() {
        var transportMock = TransportMock;
        transportMock.Setup( x => x.Open()).ReturnsAsync(StreamMock.Object);
        
        var command = new MyWriteCommand {
            Transport = transportMock.Object,
            Value = new MyParam { CommandPid = 1, Value = "test"}
        };

        MyParam? expected = null;
        command.ValueChanged += s => expected = s;

        await command.Send();
        
        Assert.Equal(expected, command.Value);
        
    }

    [Fact]
    public async void ShouldSendCompiledCommand() {
        var transportMock = TransportMock;
        transportMock.Setup( x => x.Open()).ReturnsAsync(StreamMock.Object);
        
        var command = new MyWriteCommand {
            Transport = transportMock.Object,
            Value = new MyParam { CommandPid = 1, Value = "test"}
        };
        
        await command.Send();
        
        transportMock.Verify(x => x.Write(It.IsAny<Stream>(), command.CompiledCommand, new CancellationToken()));
    }
    
}