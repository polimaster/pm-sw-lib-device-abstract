using System;
using System.Threading.Tasks;
using Moq;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Tests.Impl.Device.Settings;
using Polimaster.Device.Abstract.Tests.Impl.Device.Transport;
using Polimaster.Device.Abstract.Transport;
using Polimaster.Device.Abstract.Transport.Stream;

namespace Polimaster.Device.Abstract.Tests.Tests.Transport;

public class MyTransportTest : Mocks {
    [Fact]
    public void ShouldHaveId() {
        var client = new Mock<IClient<string>>();
        var tr = new MyTransport(client.Object, LOGGER_FACTORY);
        
        Assert.Equal($"{nameof(MyTransport)}:{client.Object}", tr.ConnectionId);
    }
    
    
    [Fact]
    public void ShouldOpen() {
        var client = new Mock<IClient<string>>();
        var tr = new MyTransport(client.Object, LOGGER_FACTORY);

        tr.Open();
        client.Verify(e => e.Open());

        client.Setup(e => e.Connected).Returns(true);
        tr.Open();
        client.Verify(e => e.Open(), Times.Once);
    }

    [Fact]
    public async Task ShouldOpenAsync() {
        var client = new Mock<IClient<string>>();
        var tr = new MyTransport(client.Object, LOGGER_FACTORY);

        await tr.OpenAsync(Token);
        client.Verify(e => e.OpenAsync(Token));
        
        client.Setup(e => e.Connected).Returns(true);
        await tr.OpenAsync(Token);
        client.Verify(e => e.OpenAsync(Token), Times.Once);
    }

    [Fact]
    public void ShouldClose() {
        var client = new Mock<IClient<string>>();
        var tr = new MyTransport(client.Object, LOGGER_FACTORY);

        tr.Close();
        client.Verify(e => e.Close());
        
        tr.Dispose();
        client.Verify(e => e.Dispose());
    }

    [Fact]
    public async Task ShouldWrite() {
        var client = new Mock<IClient<string>>();
        var stream = new Mock<IDeviceStream<string>>();
        client.Setup(e => e.GetStream()).Returns(stream.Object);
        
        var writer = new Mock<IDataWriter<MyParam>>();
        var param = new MyParam();
        
        var tr = new MyTransport(client.Object, LOGGER_FACTORY);
        await tr.Write(writer.Object, param, Token);
        
        client.Verify(e => e.GetStream());
        writer.Verify(e => e.Write(stream.Object, param, Token));
    }

    [Fact]
    public async Task ShouldRead() {
        var client = new Mock<IClient<string>>();
        var stream = new Mock<IDeviceStream<string>>();
        client.Setup(e => e.GetStream()).Returns(stream.Object);
        
        var reader = new Mock<IDataReader<MyParam>>();
        var myParam = new MyParam();
        reader.Setup(e => e.Read(stream.Object, Token)).ReturnsAsync(myParam);
        
        var tr = new MyTransport(client.Object, LOGGER_FACTORY);
        var res = await tr.Read(reader.Object, Token);
        
        Assert.Equal(myParam, res);
        client.Verify(e => e.GetStream());
        reader.Verify(e => e.Read(stream.Object, Token));
    }

    [Fact]
    public async Task ShouldExec() {
        var client = new Mock<IClient<string>>();
        var stream = new Mock<IDeviceStream<string>>();
        client.Setup(e => e.GetStream()).Returns(stream.Object);
        
        var writer = new Mock<ICommand>();
        
        var tr = new MyTransport(client.Object, LOGGER_FACTORY);
        await tr.Exec(writer.Object, Token);
        
        client.Verify(e => e.GetStream());
        writer.Verify(e => e.Exec(stream.Object, Token));
    }

    [Fact]
    public async Task ShouldResetClientOnFail() {
        var client = new Mock<IClient<string>>();
        var reader = new Mock<IDataReader<MyParam>>();
        var ex = new Exception("FAIL");
        reader.Setup(e => e.Read(It.IsAny<object>(), Token)).ThrowsAsync(ex, TimeSpan.FromSeconds(2));
        
        var tr = new MyTransport(client.Object, LOGGER_FACTORY);
        
        Exception? exception = null;
        try {
            await tr.Read(reader.Object, Token);
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