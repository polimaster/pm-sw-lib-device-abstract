using System;
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
    public async void ShouldCallValueChanged() {
        var writerMock = WriterMock;
        var readerMock = ReaderMock;

        var transportMock = TransportMock;
        transportMock.Setup(x => x.GetWriter()).Returns(writerMock.Object);
        transportMock.Setup(x => x.GetReader()).Returns(readerMock.Object);
        
        var command = new MyWriteCommand {
            Device = new MyDevice { Transport = transportMock.Object },
            Value = new MyParam { CommandPid = 1, Value = "test"}
        };

        MyParam? expected = null;
        command.ValueChanged += s => expected = s;

        await command.Send();
        
        Assert.Equal(expected, command.Value);
        
    }

}