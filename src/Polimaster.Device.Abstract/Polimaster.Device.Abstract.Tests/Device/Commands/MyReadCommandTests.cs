using System;
using System.IO;
using System.Threading;
using Moq;

namespace Polimaster.Device.Abstract.Tests.Device.Commands;

public class MyReadCommandTests : Mocks {
    [Fact]
    public async void ShouldThrowExceptionIfTransportNull() {
        var command = new MyReadCommand();

        await Assert.ThrowsAsync<NullReferenceException>(() => command.Send());
    }

    [Fact]
    public async void ShouldCancelSend() {
        var transportMock = TransportMock;
        var token = new CancellationToken(true);
        transportMock.Setup(x => x.Open()).ReturnsAsync(StreamMock.Object);

        var command = new MyReadCommand {
            Transport = transportMock.Object
        };

        await command.Send(token);

        transportMock.Verify(x => x.Write(It.IsAny<Stream>(), It.IsAny<string>(), token), Times.Never);
    }

    [Fact]
    public async void ShouldCallValueChanged() {
        var transportMock = TransportMock;
        const string readValue = "READ-VALUE";
        transportMock.Setup(x => x.Open()).ReturnsAsync(StreamMock.Object);
        transportMock.Setup(x => x.Read(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(readValue);

        var command = new MyReadCommand {
            Transport = transportMock.Object,
            Value = new MyParam { CommandPid = 1 }
        };

        MyParam? expected = null;
        command.ValueChanged += s => expected = s;

        await command.Send();

        Assert.Equal(readValue, expected?.Value);
        Assert.Equal(expected?.Value, command.Value.Value);
    }
}