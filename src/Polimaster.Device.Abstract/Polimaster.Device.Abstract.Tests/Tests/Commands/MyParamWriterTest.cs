using System.Threading.Tasks;
using Moq;
using Polimaster.Device.Abstract.Tests.Impl.Device.Commands;
using Polimaster.Device.Abstract.Tests.Impl.Device.Settings;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Tests.Commands;

public class MyParamWriterTest : Mocks {
    private readonly MyParam _param = new() { CommandPid = 1, Value = "VALUE" };

    [Fact]
    public async Task ShouldWrite() {
        var transport = new Mock<ITransport<string>>();
        var cmd = new MyParamWriter(transport.Object, LOGGER_FACTORY);

        await cmd.Write(_param, Token);

        var compiled = $"{Cmd.PREFIX}{_param.CommandPid}:{_param.Value}";
        transport.Verify(e => e.WriteAsync(compiled, Token));
    }
}