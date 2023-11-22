using System;
using System.Threading;
using Moq;
using Polimaster.Device.Abstract.Tests.Impl.Device.Commands;
using Polimaster.Device.Abstract.Tests.Impl.Device.Settings;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Tests.Commands;

public class MyParamWriterTest : Mocks {
    private readonly MyParam _param = new() { CommandPid = 1, Value = "VALUE" };
    private readonly CancellationToken _token = new();

    [Fact]
    public async void ShouldWrite() {
        var cmd = new MyParamWriter(LOGGER_FACTORY);
        var stream = new Mock<IDeviceStream<string>>();

        await cmd.Write(stream.Object, _param, _token);

        var compiled = $"{Cmd.PREFIX}{_param?.CommandPid}:{_param?.Value}";
        stream.Verify(e => e.WriteAsync(compiled, _token));
    }

    [Fact]
    public async void ShouldFailOnWrite() {
        // should throw exception because type of command and stream type is differs (string != int)
        var cmd = new MyParamWriter(LOGGER_FACTORY);
        var stream = new Mock<IDeviceStream<int>>();

        Exception? exception = null;
        try { await cmd.Write(stream.Object, _param, _token); } catch (Exception e) { exception = e; }

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
    }
}