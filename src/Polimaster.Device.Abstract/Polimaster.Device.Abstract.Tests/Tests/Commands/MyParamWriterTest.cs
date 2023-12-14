using System;
using Moq;
using Polimaster.Device.Abstract.Tests.Impl.Device.Commands;
using Polimaster.Device.Abstract.Tests.Impl.Device.Settings;
using Polimaster.Device.Abstract.Transport.Stream;

namespace Polimaster.Device.Abstract.Tests.Tests.Commands;

public class MyParamWriterTest : Mocks {
    private readonly MyParam _param = new() { CommandPid = 1, Value = "VALUE" };

    [Fact]
    public async void ShouldWrite() {
        var cmd = new MyParamWriter(LOGGER_FACTORY);
        var stream = new Mock<IDeviceStream<string>>();

        await cmd.Write(stream.Object, _param, Token);

        var compiled = $"{Cmd.PREFIX}{_param.CommandPid}:{_param.Value}";
        stream.Verify(e => e.WriteAsync(compiled, Token));
    }

    [Fact]
    public async void ShouldFailOnWrite() {
        // should throw exception because type of command and stream type is differs (string != int)
        var cmd = new MyParamWriter(LOGGER_FACTORY);
        var stream = new Mock<IDeviceStream<int>>();

        Exception? exception = null;
        try { await cmd.Write(stream.Object, _param, Token); } catch (Exception e) { exception = e; }

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
    }
}