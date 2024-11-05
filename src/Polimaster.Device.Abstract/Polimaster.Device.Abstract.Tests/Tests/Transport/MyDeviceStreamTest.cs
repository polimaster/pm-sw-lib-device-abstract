using System.Threading.Tasks;
using Moq;
using Polimaster.Device.Abstract.Tests.Impl.Device.Transport;
using Polimaster.Device.Abstract.Transport.Stream.Socket;

namespace Polimaster.Device.Abstract.Tests.Tests.Transport;

public class MyDeviceStreamTest : Mocks {
    
    [Fact]
    public async Task ShouldWrite() {
        var mock = new Mock<ISocketStream>();
        var stream = new MyDeviceStream(mock.Object, LOGGER_FACTORY);
        const string str = "TEST_0";
        
        await stream.WriteAsync(str, Token);
        
        mock.Verify(e => e.WriteAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>(), Token));
    }

    [Fact]
    public async Task ShouldRead() {
        var mock = new Mock<ISocketStream>();
        var stream = new MyDeviceStream(mock.Object, LOGGER_FACTORY);
        
        await stream.ReadAsync(Token);
        
        mock.Verify(e => e.ReadAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>(), Token));
    }
    
}