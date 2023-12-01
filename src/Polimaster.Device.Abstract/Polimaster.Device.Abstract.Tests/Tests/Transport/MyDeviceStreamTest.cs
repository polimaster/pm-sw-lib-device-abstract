using System;
using System.IO;
using Moq;
using Polimaster.Device.Abstract.Tests.Impl.Device.Transport;

namespace Polimaster.Device.Abstract.Tests.Tests.Transport;

public class MyDeviceStreamTest : Mocks {
    
    [Fact]
    public async void ShouldWrite() {
        var mock = new Mock<MemoryStream>();
        var stream = new MyDeviceStream(mock.Object, LOGGER_FACTORY);
        const string str = "TEST_0";
        
        await stream.WriteAsync(str, Token);
        
        mock.Verify(e => e.WriteAsync(It.IsAny<ReadOnlyMemory<byte>>(), Token));
    }

    [Fact]
    public async void ShouldRead() {
        var mock = new Mock<MemoryStream>();
        var stream = new MyDeviceStream(mock.Object, LOGGER_FACTORY);
        
        await stream.ReadAsync(Token);
        
        mock.Verify(e => e.ReadAsync(It.IsAny<Memory<byte>>(), Token));
    }
    
}