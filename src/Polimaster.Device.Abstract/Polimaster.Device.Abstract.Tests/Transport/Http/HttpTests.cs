using Moq;
using Polimaster.Device.Abstract.Transport;
using Polimaster.Device.Abstract.Transport.Http;

namespace Polimaster.Device.Abstract.Tests.Transport.Http;

public class HttpTests {
    private readonly Mock<IClient<HttpConnectionParams>> _clientMock;
    private const string HOST = "localhost";
    private const int PORT = 80;

    public HttpTests() {
        _clientMock = new Mock<IClient<HttpConnectionParams>>();
    }

    [Fact]
    public async void ConnectionStateTests() {
        var http = new Abstract.Transport.Http.Http(_clientMock.Object, new HttpConnectionParams { Ip = HOST, Port = PORT });

        _clientMock.Setup(s => s.Connected).Returns(false);

        Assert.False(http.Client.Connected);
        // Assert.Equal(ConnectionState.Closed, mockState);

        await http.Open();

        _clientMock.Setup(s => s.Connected).Returns(true);

        Assert.True(http.Client.Connected);
        // Assert.Equal(ConnectionState.Open, mockState);
    }


    [Fact]
    public async void ShouldOpenConnection() {
        var httpConnectionParams = new HttpConnectionParams { Ip = HOST, Port = PORT };
        var http = new Abstract.Transport.Http.Http(_clientMock.Object, httpConnectionParams);
        await http.Open();

        _clientMock.Verify(v => v.ConnectAsync(httpConnectionParams));
    }

    [Fact]
    public async void ShouldCloseConnection() {
        var http = new Abstract.Transport.Http.Http(_clientMock.Object,
            new HttpConnectionParams { Ip = HOST, Port = PORT });
        await http.Close();

        _clientMock.Verify(v => v.Close());
    }

    [Fact]
    public void ShouldDisposeTcpClient() {
        var http = new Abstract.Transport.Http.Http(_clientMock.Object,
            new HttpConnectionParams { Ip = HOST, Port = PORT });
        http.Dispose();

        _clientMock.Verify(v => v.Dispose());
    }

    // [Fact]
    // public async void ShouldWrite() {
    //     var http = new Http<ITcpClient>(_clientMock.Object);
    //
    //     var streamMock = new Mock<Stream>();
    //     streamMock.Setup(v => v.CanWrite).Returns(true);
    //     
    //     _clientMock.Setup(s => s.GetStream()).Returns(streamMock.Object);
    //
    //     await http.Write(streamMock.Object, string.Empty, CancellationToken.None);
    //     
    //     _clientMock.Verify(v => v.GetStream());
    // }

    // [Fact]
    // public async void ShouldRead() {
    //     var http = new Http<ITcpClient>(_clientMock.Object);
    //
    //     var streamMock = new Mock<Stream>();
    //     streamMock.Setup(v => v.CanWrite).Returns(true);
    //     streamMock.Setup(v => v.CanRead).Returns(true);
    //     
    //     _clientMock.Setup(s => s.GetStream()).Returns(streamMock.Object);
    //     
    //     await http.Read(streamMock.Object, "", CancellationToken.None);
    //     
    //     _clientMock.Verify(v => v.GetStream());
    // }
}