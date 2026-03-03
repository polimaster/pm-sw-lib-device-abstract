using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Polimaster.Device.Abstract.Tests.Impl.Transport;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Tests.Transport;

public class ATransportTest : Mocks {
    [Fact]
    public async Task ShouldOpenAndClose() {
        var client = new Mock<IClient<IMyDeviceStream>>();
        client.Setup(c => c.Connected).Returns(false);
        var transport = new MySimpleTransport(client.Object, LOGGER_FACTORY);

        await transport.Open(Token);
        client.Verify(c => c.Open(Token), Times.Once);

        transport.Close();
        client.Verify(c => c.Close(), Times.Once);
    }

    [Fact]
    public async Task ShouldExecOnStream() {
        var client = new Mock<IClient<IMyDeviceStream>>();
        var stream = new Mock<IMyDeviceStream>();
        client.Setup(c => c.GetStream()).Returns(stream.Object);
        var transport = new MySimpleTransport(client.Object, LOGGER_FACTORY);

        var executed = false;
        await transport.ExecOnStream(s => {
            executed = true;
            return Task.CompletedTask;
        }, Token);

        Assert.True(executed);
        client.Verify(c => c.Open(Token), Times.Once);
    }

    [Fact]
    public void ShouldRaiseClosingEvent() {
        var client = new Mock<IClient<IMyDeviceStream>>();
        var transport = new MySimpleTransport(client.Object, LOGGER_FACTORY);
        var raised = false;
        transport.Closing += () => raised = true;

        transport.Close();
        Assert.True(raised);
    }

    [Fact]
    public void ShouldDisposeClient() {
        var client = new Mock<IClient<IMyDeviceStream>>();
        var transport = new MySimpleTransport(client.Object, LOGGER_FACTORY);

        transport.Dispose();
        client.Verify(c => c.Dispose(), Times.Once);
    }

    private class MySimpleTransport : ATransport<IMyDeviceStream> {
        public MySimpleTransport(IClient<IMyDeviceStream> client, ILoggerFactory? loggerFactory) : base(client, loggerFactory) {
        }
    }
}
