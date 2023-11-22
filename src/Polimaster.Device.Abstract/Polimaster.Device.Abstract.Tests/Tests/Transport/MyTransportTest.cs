using System.Threading;
using Moq;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Tests.Impl.Device.Settings;
using Polimaster.Device.Abstract.Tests.Impl.Device.Transport;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Tests.Transport;

public class MyTransportTest : Mocks {
    private readonly CancellationToken _token = new();

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

        await tr.OpenAsync();
        client.Verify(e => e.OpenAsync());
        
        client.Setup(e => e.Connected).Returns(true);
        await tr.OpenAsync();
        client.Verify(e => e.OpenAsync(), Times.Once);
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
        await tr.Write(writer.Object, param, _token);
        
        writer.Verify(e => e.Write(stream.Object, param, _token));
    }

    [Fact]
    public async void ShouldRead() {
        var client = new Mock<IClient<string>>();
        var stream = new Mock<IDeviceStream<string>>();
        client.Setup(e => e.GetStream()).Returns(stream.Object);
        
        var writer = new Mock<IDataReader<MyParam>>();
        
        var tr = new MyTransport(client.Object, LOGGER_FACTORY);
        await tr.Read(writer.Object, _token);
        
        writer.Verify(e => e.Read(stream.Object, _token));
    }

    [Fact]
    public async void ShouldExec() {
        var client = new Mock<IClient<string>>();
        var stream = new Mock<IDeviceStream<string>>();
        client.Setup(e => e.GetStream()).Returns(stream.Object);
        
        var writer = new Mock<ICommand>();
        
        var tr = new MyTransport(client.Object, LOGGER_FACTORY);
        await tr.Exec(writer.Object, _token);
        
        writer.Verify(e => e.Exec(stream.Object, _token));
    }
}