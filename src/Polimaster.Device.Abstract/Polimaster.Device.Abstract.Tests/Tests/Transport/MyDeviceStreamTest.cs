using System.Threading.Tasks;
using Moq;
using Polimaster.Device.Abstract.Tests.Impl.Transport;
using Polimaster.Device.Abstract.Tests.Impl.Transport.Socket;

namespace Polimaster.Device.Abstract.Tests.Tests.Transport;

public class MyDeviceStreamTest : Mocks {
    
    [Fact]
    public async Task ShouldWrite() {
        var mock = new Mock<ISocketStream>();
        var stream = new MyDeviceStream(mock.Object, LOGGER_FACTORY);
        var str = "TEST_0"u8.ToArray();
        
        await stream.Write(str, Token);
        
        mock.Verify(e => e.WriteAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>(), Token));
    }

    [Fact]
    public async Task ShouldRead() {
        var mock = new Mock<ISocketStream>();
        var stream = new MyDeviceStream(mock.Object, LOGGER_FACTORY);
        
        await stream.Read(Token);
        
        mock.Verify(e => e.ReadAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>(), Token));
    }
    
}