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
    public async void ShouldOpenAsync() {
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
    public async void ShouldWrite() {
        var client = new Mock<IClient<string>>();
        var stream = new Mock<IDeviceStream<string>>();
        client.Setup(e => e.GetStream()).Returns(stream.Object);
        
        var writer = new Mock<IDataWriter<MyParam>>();
        var param = new MyParam();
        
        var tr = new MyTransport(client.Object, LOGGER_FACTORY);
        await tr.Write(writer.Object, param, Token);
        
        writer.Verify(e => e.Write(stream.Object, param, Token));
    }

    [Fact]
    public async void ShouldRead() {
        var client = new Mock<IClient<string>>();
        var stream = new Mock<IDeviceStream<string>>();
        client.Setup(e => e.GetStream()).Returns(stream.Object);
        
        var writer = new Mock<IDataReader<MyParam>>();
        
        var tr = new MyTransport(client.Object, LOGGER_FACTORY);
        await tr.Read(writer.Object, Token);
        
        writer.Verify(e => e.Read(stream.Object, Token));
    }

    [Fact]
    public async void ShouldExec() {
        var client = new Mock<IClient<string>>();
        var stream = new Mock<IDeviceStream<string>>();
        client.Setup(e => e.GetStream()).Returns(stream.Object);
        
        var writer = new Mock<ICommand>();
        
        var tr = new MyTransport(client.Object, LOGGER_FACTORY);
        await tr.Exec(writer.Object, Token);
        
        writer.Verify(e => e.Exec(stream.Object, Token));
    }
}