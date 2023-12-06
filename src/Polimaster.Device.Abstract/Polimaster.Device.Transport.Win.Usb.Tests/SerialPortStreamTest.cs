using System;
using System.IO.Ports;
using Moq;

namespace Polimaster.Device.Transport.Win.Usb.Tests;

public class SerialPortStreamTest : Mocks {
    
    [Fact]
    public async void ShouldRead() {
        var port = new Mock<SerialPort>();
        var response = Guid.NewGuid().ToString();
        port.Setup(e => e.ReadTo(It.IsAny<string>())).Returns(response);
        var stream = new SerialPortStream(port.Object, LOGGER_FACTORY);

        var result = await stream.ReadAsync(Token);
        
        port.Verify(e => e.ReadTo(It.IsAny<string>()));
        Assert.Equal(response, result);
    }
    
}