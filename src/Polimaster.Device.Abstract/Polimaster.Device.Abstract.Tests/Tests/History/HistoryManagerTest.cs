using System.Threading;
using System.Threading.Tasks;
using Moq;
using Polimaster.Device.Abstract.Device.Implementations.History;
using Polimaster.Device.Abstract.Tests.Impl.Device.Commands;
using Polimaster.Device.Abstract.Tests.Impl.Device.History;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Tests.History;

public class HistoryManagerTest : Mocks {
    [Fact]
    public async void ShouldReadHistory() {
        var transport = new Mock<ITransport>();
        transport.Setup(e => e.Read(It.IsAny<HistoryReader>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new HistoryReaderChunk { HasReachedTheEnd = true }));

        HistoryChunk<HistoryRecord>? res = null;
        var hm = new HistoryManager(LOGGER_FACTORY);
        hm.HasNext += chunk => {
            res = chunk;
        };

        await hm.Read(transport.Object, Token);

        transport.Verify(e => e.Read(It.IsAny<HistoryReader>(), It.IsAny<CancellationToken>()));
        Assert.NotNull(res);
    }

    [Fact]
    public async void ShouldWipeHistory() {
        var transport = new Mock<ITransport>();
        var hm = new HistoryManager(LOGGER_FACTORY);

        await hm.Wipe(transport.Object, Token);
        
        transport.Verify(e => e.Exec(It.IsAny<HistoryWiper>(), Token));
    }
}