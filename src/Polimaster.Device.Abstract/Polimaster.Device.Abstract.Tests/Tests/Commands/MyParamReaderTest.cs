using System.Threading.Tasks;
using Moq;
using Polimaster.Device.Abstract.Tests.Impl.Commands;
using Polimaster.Device.Abstract.Tests.Impl.Transport;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Tests.Commands; 

public class ADataReaderTest : Mocks {
    
    [Fact]
    public async Task ShouldRead() {
        var result = "CMD=123:456"u8.ToArray();
        var client = new Mock<IClient<IMyDeviceStream>>();
        var stream = new Mock<IMyDeviceStream>();
        stream.Setup(e => e.Read(Token)).Returns(Task.FromResult(result));
        client.Setup(e => e.GetStream()).Returns(stream.Object);
        var transport = new MyTransport(client.Object, LOGGER_FACTORY);
        // transport.Setup(e => e.ReadAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult("CMD=123:456"u8.ToArray()));

        var cmd = new MyParamReader(transport, LOGGER_FACTORY);
        await cmd.Read(Token);

        stream.Verify(e => e.Read(Token));
    }
}