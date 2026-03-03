using System.Threading.Tasks;
using Moq;
using Polimaster.Device.Abstract.Tests.Impl.Device;
using Polimaster.Device.Abstract.Tests.Impl.Transport;

namespace Polimaster.Device.Abstract.Tests.Tests.Device;

public class ADeviceStatusTest : Mocks {
    [Fact]
    public async Task ShouldReadStatus() {
        var transport = new Mock<IMyTransport>();
        var status = new MyDeviceStatus(transport.Object, LOGGER_FACTORY);

        var res = await status.Read(Token);
        
        Assert.Equal(42, res.Value);
    }

    [Fact]
    public void ShouldStartAndStop() {
        var transport = new Mock<IMyTransport>();
        var status = new MyDeviceStatus(transport.Object, LOGGER_FACTORY);

        status.Start(Token);
        Assert.True(status.IsStarted);

        status.Stop();
        Assert.True(status.IsStopped);
    }

    [Fact]
    public void ShouldStopOnTransportClosing() {
        var transport = new Mock<IMyTransport>();
        var status = new MyDeviceStatus(transport.Object, LOGGER_FACTORY);

        transport.Raise(t => t.Closing += null);
        
        Assert.True(status.IsStopped);
    }

    [Fact]
    public void ShouldHandleHasNext() {
        var transport = new Mock<IMyTransport>();
        var status = new MyDeviceStatus(transport.Object, LOGGER_FACTORY);
        MyStatus? received = null;
        status.HasNext += s => received = s;

        var expected = new MyStatus { Value = 100 };
        status.RaiseHasNext(expected);

        Assert.Equal(expected, received);
    }
}
