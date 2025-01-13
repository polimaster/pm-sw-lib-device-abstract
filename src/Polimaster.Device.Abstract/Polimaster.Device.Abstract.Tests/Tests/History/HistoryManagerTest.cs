using System.Threading;
using System.Threading.Tasks;
using Moq;
using Polimaster.Device.Abstract.Device.Implementations.History;
using Polimaster.Device.Abstract.Tests.Impl.Device.History;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Tests.History;

public class HistoryManagerTest : Mocks {
    [Fact]
    public async Task ShouldReadHistory() {
        var transport = new Mock<ITransport<string>>();
        transport.Setup(e => e.ReadAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(string.Empty));

        HistoryChunk<HistoryRecord>? res = null;
        var hm = new HistoryManager(transport.Object, LOGGER_FACTORY);
        hm.HasNext += chunk => {
            res = chunk;
        };

        await hm.Read(Token);

        transport.Verify(e => e.ReadAsync(It.IsAny<CancellationToken>()));
        Assert.NotNull(res);
    }

    [Fact]
    public async Task ShouldWipeHistory() {
        var transport = new Mock<ITransport<string>>();
        var hm = new HistoryManager(transport.Object, LOGGER_FACTORY);

        await hm.Wipe(Token);
        
        transport.Verify(e => e.WriteAsync(It.IsAny<string>(), Token));
    }
}