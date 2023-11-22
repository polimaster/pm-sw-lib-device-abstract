using System;
using System.IO;
using System.Threading;
using Moq;
using Polimaster.Device.Abstract.Tests.Impl.Device.Transport;

namespace Polimaster.Device.Abstract.Tests.Tests.Transport;

public class MyDeviceStreamTest : Mocks {
    private readonly CancellationToken _token = new();
    
    [Fact]
    public async void ShouldWrite() {
        var mock = new Mock<MemoryStream>();
        var stream = new MyDeviceStream(mock.Object, LOGGER_FACTORY);
        const string str = "TEST_0";
        
        await stream.WriteAsync(str, _token);
        
        mock.Verify(e => e.WriteAsync(It.IsAny<ReadOnlyMemory<byte>>(), _token));
    }

    [Fact]
    public async void ShouldRead() {
        var mock = new Mock<MemoryStream>();
        var stream = new MyDeviceStream(mock.Object, LOGGER_FACTORY);
        
        await stream.ReadAsync(_token);
        
        mock.Verify(e => e.ReadAsync(It.IsAny<Memory<byte>>(), _token));
    }
    
}