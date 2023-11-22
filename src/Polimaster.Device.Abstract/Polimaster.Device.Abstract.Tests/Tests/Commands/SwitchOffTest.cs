using System;
using System.Threading;
using Moq;
using Polimaster.Device.Abstract.Tests.Impl.Device.Commands;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Tests.Commands;

public class SwitchOffTest : Mocks {
    private readonly CancellationToken _token = new();
    
    [Fact]
    public async void ShouldExec() {
        var cmd = new SwitchOff(LOGGER_FACTORY);
        var stream = new Mock<IDeviceStream<string>>();

        await cmd.Exec(stream.Object, _token);

        stream.Verify(e => e.WriteAsync(cmd.Compiled, _token));
    }

    [Fact]
    public async void ShouldFailExec() {
        // should throw exception because type of command and stream type is differs (string != int)
        var cmd = new SwitchOff(LOGGER_FACTORY);
        var stream = new Mock<IDeviceStream<int>>();

        Exception? exception = null;
        try { await cmd.Exec(stream.Object, _token); } catch (Exception e) { exception = e; }

        Assert.NotNull(exception);
        Assert.True(exception.GetType() == typeof(ArgumentException));
    }
}