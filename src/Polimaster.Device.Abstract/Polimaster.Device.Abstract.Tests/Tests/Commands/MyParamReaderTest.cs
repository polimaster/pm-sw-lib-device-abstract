using System;
using System.Threading;
using Moq;
using Polimaster.Device.Abstract.Tests.Impl.Device.Commands;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Tests.Commands; 

public class MyParamReaderTest : Mocks {
    private readonly CancellationToken _token = new();
    
    [Fact]
    public async void ShouldRead() {
        var cmd = new MyParamReader(LOGGER_FACTORY);
        var stream = new Mock<IDeviceStream<string>>();

        var res = await cmd.Read(stream.Object, _token);
        
        stream.Verify(e => e.ReadAsync(_token));
    }
    
    [Fact]
    public async void ShouldFailOnWrite() {
        // should throw exception because type of command and stream type is differs (string != int)
        var cmd = new MyParamReader(LOGGER_FACTORY);
        var stream = new Mock<IDeviceStream<int>>();

        Exception? exception = null;
        try { await cmd.Read(stream.Object, _token); } catch (Exception e) { exception = e; }

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
    }
    
}