using System;
using Moq;

namespace Polimaster.Device.Abstract.Tests.Device.Commands;

public class MyReadCommandTests : Mocks {
    [Fact]
    public async void ShouldThrowExceptionIfTransportNull() {
        var command = new MyReadCommand();

        await Assert.ThrowsAsync<NullReferenceException>(() => command.Send());
    }

    [Fact]
    public async void ShouldCallValueChanged() {
        
        const string readValue = "READ-VALUE";
        var writerMock = WriterMock;
        var readerMock = ReaderMock;
        readerMock.Setup(e => e.ReadToEndAsync()).ReturnsAsync(readValue);
        
        var transportMock = TransportMock;
        transportMock.Setup(x => x.GetWriter()).ReturnsAsync(writerMock.Object);
        transportMock.Setup(x => x.GetReader()).ReturnsAsync(readerMock.Object);

        var command = new MyReadCommand {
            Device = new MyDevice { Transport = transportMock.Object },
            Value = new MyParam { CommandPid = 1 }
        };

        MyParam? expected = null;
        command.ValueChanged += s => expected = s;

        await command.Send();

        Assert.Equal(readValue, expected?.Value);
        Assert.Equal(expected?.Value, command.Value.Value);
    }
}