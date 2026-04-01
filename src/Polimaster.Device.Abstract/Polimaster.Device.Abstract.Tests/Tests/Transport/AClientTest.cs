using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Tests.Transport;

public class AClientTest : Mocks {
    private class TestClient : AClient<string, int> {
        public TestClient(int @params, ILoggerFactory? loggerFactory) : base(@params, loggerFactory) { }
        public override void Dispose() { }
        public override bool Connected => false;
        public override void Close() { }
        public override Task Open(CancellationToken token) => Task.CompletedTask;
        public override string GetStream() => "stream";
        public override void Reset() { }
    }

    [Fact]
    public void ShouldHaveCorrectConnectionId() {
        var client = new TestClient(123, LOGGER_FACTORY);
        Assert.Equal("TestClient#123", client.ConnectionId);
    }

    [Fact]
    public void ShouldBeEqualByConnectionId() {
        var client1 = new TestClient(123, LOGGER_FACTORY);
        var client2 = new Mock<IClient<string>>();
        client2.Setup(e => e.ConnectionId).Returns("TestClient#123");

        Assert.True(client1.Equals(client2.Object));
    }
}
