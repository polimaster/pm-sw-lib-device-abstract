using System;
using System.Threading.Tasks;
using Moq;
using Polimaster.Device.Abstract.Tests.Impl.Device.Transport;
using Polimaster.Device.Abstract.Transport;
using Polimaster.Device.Abstract.Transport.Stream;

namespace Polimaster.Device.Abstract.Tests.Tests.Transport;

public class MyTransportTest : Mocks {
    
    [Fact]
    public void ShouldOpen() {
        var client = new Mock<IClient>();
        var tr = new MyTransport(client.Object, LOGGER_FACTORY);

        tr.Open();
        client.Verify(e => e.Open());

        client.Setup(e => e.Connected).Returns(true);

        tr.Open();
        client.Verify(e => e.Open(), Times.Once);
    }

    [Fact]
    public async Task ShouldOpenAsync() {
        var client = new Mock<IClient>();
        var tr = new MyTransport(client.Object, LOGGER_FACTORY);

        await tr.OpenAsync(Token);
        client.Verify(e => e.OpenAsync(Token));
        
        client.Setup(e => e.Connected).Returns(true);
        await tr.OpenAsync(Token);
        client.Verify(e => e.OpenAsync(Token), Times.Once);
    }

    [Fact]
    public void ShouldClose() {
        var client = new Mock<IClient>();
        var tr = new MyTransport(client.Object, LOGGER_FACTORY);

        tr.Close();
        client.Verify(e => e.Close());
        
        tr.Dispose();
        client.Verify(e => e.Dispose());
    }

    [Fact]
    public async Task ShouldWrite() {
        var client = new Mock<IClient>();
        var stream = new Mock<IDeviceStream>();
        client.Setup(e => e.GetStream()).Returns(stream.Object);

        var param = Guid.NewGuid().ToByteArray();
        
        var tr = new MyTransport(client.Object, LOGGER_FACTORY);
        await tr.WriteAsync<object>(param, Token);
        
        stream.Verify(e => e.WriteAsync(param, Token, It.IsAny<object>()));
    }

    [Fact]
    public async Task ShouldRead() {
        var client = new Mock<IClient>();
        var stream = new Mock<IDeviceStream>();
        client.Setup(e => e.GetStream()).Returns(stream.Object);
        
        var tr = new MyTransport(client.Object, LOGGER_FACTORY);
        await tr.ReadAsync(Token);

        stream.Verify(e => e.ReadAsync(Token, It.IsAny<object>()));
    }

    [Fact]
    public async Task ShouldResetClientOnFail() {
        var client = new Mock<IClient>();
        var stream = new Mock<IDeviceStream>();
        client.Setup(e => e.GetStream()).Returns(stream.Object);

        var ex = new Exception("FAIL");
        stream.Setup(e => e.ReadAsync(Token, It.IsAny<object>())).ThrowsAsync(ex, TimeSpan.FromSeconds(2));
        
        var tr = new MyTransport(client.Object, LOGGER_FACTORY);
        
        Exception? exception = null;
        try {
            await tr.ReadAsync(Token);
        } catch (Exception e) {
            exception = e;
        }
        
        Assert.NotNull(exception);
        Assert.Equal(ex, exception);
        client.Verify(e => e.Reset());
        client.Verify(e => e.OpenAsync(Token));
        client.Verify(e => e.GetStream(), Times.Exactly(2));
    }
}