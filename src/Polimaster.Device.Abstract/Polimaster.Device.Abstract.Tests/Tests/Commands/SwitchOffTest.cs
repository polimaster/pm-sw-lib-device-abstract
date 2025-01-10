using System.Threading.Tasks;
using Moq;
using Polimaster.Device.Abstract.Tests.Impl.Device.Commands;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Tests.Commands;

public class SwitchOffTest : Mocks {
    
    [Fact]
    public async Task ShouldExec() {
        var transport = new Mock<ITransport<string>>();
        var cmd = new SwitchOff(transport.Object, LOGGER_FACTORY);

        await cmd.Exec(Token);

        transport.Verify(e => e.WriteAsync(cmd.Compiled, Token));
    }
}