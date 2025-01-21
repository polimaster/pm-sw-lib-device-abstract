using System.Threading;
using System.Threading.Tasks;
using Moq;
using Polimaster.Device.Abstract.Device.Implementations.History;
using Polimaster.Device.Abstract.Tests.Impl.Commands;
using Polimaster.Device.Abstract.Tests.Impl.History;
using Polimaster.Device.Abstract.Tests.Impl.Transport;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Tests.History;

public class AHistoryManagerTest : Mocks {

    [Fact]
    public async Task ShouldReadHistory() {
        byte[] result = [];
        var client = new Mock<IClient<IMyDeviceStream>>();
        var stream = new Mock<IMyDeviceStream>();
        stream.Setup(e => e.Read(Token)).Returns(Task.FromResult(result));
        client.Setup(e => e.GetStream()).Returns(stream.Object);
        var transport = new MyTransport(client.Object, LOGGER_FACTORY);

        HistoryChunk<HistoryRecord>? res = null;
        var hm = new HistoryManager(transport, LOGGER_FACTORY);
        hm.HasNext += chunk => {
            res = chunk;
        };

        await hm.Read(Token);

        // transport.Verify(e => e.ReadAsync(It.IsAny<CancellationToken>()));
        client.Verify(e => e.GetStream(), Times.Once);
        stream.Verify(e => e.Read(It.IsAny<CancellationToken>()), Times.Once);
        Assert.NotNull(res);
    }

    [Fact]
    public async Task ShouldWipeHistory() {
        var client = new Mock<IClient<IMyDeviceStream>>();
        var stream = new Mock<IMyDeviceStream>();
        client.Setup(e => e.GetStream()).Returns(stream.Object);
        var transport = new MyTransport(client.Object, LOGGER_FACTORY);

        var hm = new HistoryManager(transport, LOGGER_FACTORY);

        await hm.Wipe(Token);
        
        stream.Verify(e => e.Write(HistoryWiper.COMMAND, Token));
    }
}