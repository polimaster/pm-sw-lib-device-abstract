using System.Threading;
using System.Threading.Tasks;
using Moq;
using Polimaster.Device.Abstract.Tests.Impl.Device.Commands;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Tests.Commands; 

public class MyParamReaderTest : Mocks {
    
    [Fact]
    public async Task ShouldRead() {
        var transport = new Mock<ITransport>();
        var cmd = new MyParamReader(transport.Object, LOGGER_FACTORY);

        transport.Setup(e => e.ReadAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult("CMD=123:456"u8.ToArray()));

        await cmd.Read(Token);
        transport.Verify(e => e.ReadAsync(Token));
    }
}