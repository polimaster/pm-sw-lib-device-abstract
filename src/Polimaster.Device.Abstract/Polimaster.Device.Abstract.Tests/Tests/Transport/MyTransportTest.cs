using System;
using System.Threading.Tasks;
using Moq;
using Polimaster.Device.Abstract.Tests.Impl.Transport;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Tests.Transport;

public class MyTransportTest : Mocks {
    
    [Fact]
    public async Task ShouldOpen() {
        var client = new Mock<IClient<IMyDeviceStream>>();
        var tr = new MyTransport(client.Object, LOGGER_FACTORY);

        await tr.Open(Token);
        client.Verify(e => e.Open(Token));

        client.Setup(e => e.Connected).Returns(true);

        await tr.Open(Token);
        client.Verify(e => e.Open(Token), Times.Once);
    }

    [Fact]
    public void ShouldClose() {
        var client = new Mock<IClient<IMyDeviceStream>>();
        var tr = new MyTransport(client.Object, LOGGER_FACTORY);

        tr.Close();
        client.Verify(e => e.Close());
        
        tr.Dispose();
        client.Verify(e => e.Dispose());
    }

    [Fact]
    public async Task ShouldWrite() {
        var client = new Mock<IClient<IMyDeviceStream>>();
        var stream = new Mock<IMyDeviceStream>();
        client.Setup(e => e.Connected).Returns(true);
        client.Setup(e => e.GetStream()).Returns(stream.Object);

        var param = Guid.NewGuid().ToByteArray();
        
        var tr = new MyTransport(client.Object, LOGGER_FACTORY);
        await tr.ExecOnStream(async s => {
            await s.Write(param, Token);
        }, Token);
        
        stream.Verify(e => e.Write(param, Token));
    }

    [Fact]
    public async Task ShouldRead() {
        var client = new Mock<IClient<IMyDeviceStream>>();
        var stream = new Mock<IMyDeviceStream>();
        client.Setup(e => e.Connected).Returns(true);
        client.Setup(e => e.GetStream()).Returns(stream.Object);
        
        var tr = new MyTransport(client.Object, LOGGER_FACTORY);
        await tr.ExecOnStream(async s => {
            await s.Read(Token);
        }, Token);

        stream.Verify(e => e.Read(Token));
    }

    [Fact]
    public async Task ShouldResetClientOnFail() {
        var client = new Mock<IClient<IMyDeviceStream>>();
        var stream = new Mock<IMyDeviceStream>();
        client.Setup(e => e.Connected).Returns(true);
        client.Setup(e => e.GetStream()).Returns(stream.Object);

        var ex = new Exception("FAIL");
        stream.Setup(e => e.Read(Token)).ThrowsAsync(ex, TimeSpan.FromSeconds(2));
        
        var tr = new MyTransport(client.Object, LOGGER_FACTORY);
        
        Exception? exception = null;
        try {
            await tr.ExecOnStream(async s => {
                await s.Read(Token);
            }, Token);
        } catch (Exception e) {
            exception = e;
        }
        
        Assert.NotNull(exception);
        Assert.Equal(ex, exception);
        client.Verify(e => e.Reset());
        client.Verify(e => e.GetStream(), Times.Exactly(2));
    }
}