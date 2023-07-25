using System;
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
    public async void ShouldCallValueChanged() {
        
        const string readValue = "READ-VALUE";
        var writerMock = DeviceStreamMock;
        writerMock.Setup(e => e.ReadLineAsync(It.IsAny<CancellationToken>())).ReturnsAsync(readValue);
        
        var transportMock = TransportMock;
        transportMock.Setup(x => x.Open()).ReturnsAsync(writerMock.Object);

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