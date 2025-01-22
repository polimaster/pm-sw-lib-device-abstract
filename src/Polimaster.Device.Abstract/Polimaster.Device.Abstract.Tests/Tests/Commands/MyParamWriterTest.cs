using System.Text;
using System.Threading.Tasks;
using Moq;
using Polimaster.Device.Abstract.Tests.Impl.Commands;
using Polimaster.Device.Abstract.Tests.Impl.Settings;
using Polimaster.Device.Abstract.Tests.Impl.Transport;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Tests.Commands;

public class AWriterTest : Mocks {
    private readonly MyParam _param = new() { CommandPid = 1, Value = "VALUE" };

    [Fact]
    public async Task ShouldWrite() {
        var client = new Mock<IClient<IMyDeviceStream>>();
        var stream = new Mock<IMyDeviceStream>();
        client.Setup(e => e.GetStream()).Returns(stream.Object);
        var transport = new MyTransport(client.Object, LOGGER_FACTORY);

        var cmd = new MyParamWriter(transport, LOGGER_FACTORY);

        await cmd.Write(_param, Token);

        var compiled = Encoding.UTF8.GetBytes($"{Cmd.PREFIX}{_param.CommandPid}:{_param.Value}");
        stream.Verify(e => e.Write(compiled, Token));
    }
}