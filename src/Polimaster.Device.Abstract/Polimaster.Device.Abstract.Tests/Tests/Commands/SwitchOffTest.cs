using System.Text;
using System.Threading.Tasks;
using Moq;
using Polimaster.Device.Abstract.Tests.Impl.Commands;
using Polimaster.Device.Abstract.Tests.Impl.Transport;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Tests.Commands;

public class ACommandTest : Mocks {
    
    [Fact]
    public async Task ShouldExec() {
        var client = new Mock<IClient<IMyDeviceStream>>();
        var stream = new Mock<IMyDeviceStream>();
        client.Setup(e => e.GetStream()).Returns(stream.Object);
        var transport = new MyTransport(client.Object, LOGGER_FACTORY);

        var cmd = new SwitchOff(transport, LOGGER_FACTORY);

        await cmd.Exec(Token);

        var str = Encoding.UTF8.GetBytes($"{Cmd.PREFIX}SWITCH_OFF");

        stream.Verify(e => e.Write(str, Token));
    }
}