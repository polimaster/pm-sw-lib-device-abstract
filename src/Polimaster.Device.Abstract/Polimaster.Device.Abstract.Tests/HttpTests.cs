using System.Data;
using Moq;
using Polimaster.Device.Abstract.Transport.Http;

namespace Polimaster.Device.Abstract.Tests;

public class HttpTests {
    private readonly Mock<ITcpClient> _clientMock;
    private const string HOST = "localhost";
    private const int PORT = 80;

    public HttpTests() {
        _clientMock = new Mock<ITcpClient>();
    }

    [Fact]
    public async void ConnectionStateTests() {
        var http = new Http<ITcpClient>(_clientMock.Object);
        // var mockState = ConnectionState.Closed;
        // http.ConnectionStateChanged += state => mockState = state;
        
        _clientMock.Setup(s => s.Connected).Returns(false);

        Assert.Equal(ConnectionState.Closed, http.ConnectionState);
        // Assert.Equal(ConnectionState.Closed, mockState);

        await http.Open(new HttpConnectionParams{ Ip = HOST, Port = PORT });
        
        _clientMock.Setup(s => s.Connected).Returns(true);
        
        Assert.Equal(ConnectionState.Open, http.ConnectionState);
        // Assert.Equal(ConnectionState.Open, mockState);
    }


    [Fact]
    public async void ShouldOpenConnection() {

        var http = new Http<ITcpClient>(_clientMock.Object);
        await http.Open(new HttpConnectionParams{ Ip = HOST, Port = PORT});
     
        _clientMock.Verify(v => v.ConnectAsync(HOST, PORT));
    }
    
    [Fact]
    public async void ShouldCloseConnection() {
        var http = new Http<ITcpClient>(_clientMock.Object);
        await http.Close();
     
        _clientMock.Verify(v => v.Close());
    }

    [Fact]
    public void ShouldDisposeTcpClient() {
        var http = new Http<ITcpClient>(_clientMock.Object);
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