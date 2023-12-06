using System;
using Moq;
using Polimaster.Device.Abstract.Tests.Impl.Device.Commands;
using Polimaster.Device.Abstract.Transport.Stream;

namespace Polimaster.Device.Abstract.Tests.Tests.Commands; 

public class MyParamReaderTest : Mocks {
    
    [Fact]
    public async void ShouldRead() {
        var cmd = new MyParamReader(LOGGER_FACTORY);
        var stream = new Mock<IDeviceStream<string>>();

        await cmd.Read(stream.Object, Token);
        
        stream.Verify(e => e.ReadAsync(Token));
    }
    
    [Fact]
    public async void ShouldFailOnWrite() {
        // should throw exception because type of command and stream type is differs (string != int)
        var cmd = new MyParamReader(LOGGER_FACTORY);
        var stream = new Mock<IDeviceStream<int>>();

        Exception? exception = null;
        try { await cmd.Read(stream.Object, Token); } catch (Exception e) { exception = e; }

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
    }
    
}