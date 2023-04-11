using System.Data;
using System.IO;
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
        var http = new Http<ITcpClient>(_clientMock.Object, It.IsAny<string>(), It.IsAny<int>());
        
        _clientMock.Setup(s => s.Connected).Returns(false);
        Assert.Equal(ConnectionState.Closed, http.ConnectionState);
        
        await http.Open();
        _clientMock.Setup(s => s.Connected).Returns(true);
        Assert.Equal(ConnectionState.Open, http.ConnectionState);
    }


    [Fact]
    public async void ShouldOpenConnection() {

        var http = new Http<ITcpClient>(_clientMock.Object, HOST, PORT);
        await http.Open();
     
        _clientMock.Verify(v => v.ConnectAsync(HOST, PORT));
    }
    
    [Fact]
    public async void ShouldCloseConnection() {
        var http = new Http<ITcpClient>(_clientMock.Object, It.IsAny<string>(), It.IsAny<int>());
        await http.Close();
     
        _clientMock.Verify(v => v.Close());
    }

    [Fact]
    public void ShouldDisposeTcpClient() {
        var http = new Http<ITcpClient>(_clientMock.Object, It.IsAny<string>(), It.IsAny<int>());
        http.Dispose();
        
        _clientMock.Verify(v => v.Dispose());
    }
    
    [Fact]
    public async void ShouldWrite() {
        var http = new Http<ITcpClient>(_clientMock.Object, It.IsAny<string>(), It.IsAny<int>());

        var streamMock = new Mock<Stream>();
        streamMock.Setup(v => v.CanWrite).Returns(true);
        
        _clientMock.Setup(s => s.GetStream()).Returns(streamMock.Object);
        
        await http.Write("");
        
        _clientMock.Verify(v => v.GetStream());
    }
    
    [Fact]
    public async void ShouldRead() {
        var http = new Http<ITcpClient>(_clientMock.Object, It.IsAny<string>(), It.IsAny<int>());

        var streamMock = new Mock<Stream>();
        streamMock.Setup(v => v.CanWrite).Returns(true);
        streamMock.Setup(v => v.CanRead).Returns(true);
        
        _clientMock.Setup(s => s.GetStream()).Returns(streamMock.Object);
        
        await http.Read("");
        
        _clientMock.Verify(v => v.GetStream());
    }
}