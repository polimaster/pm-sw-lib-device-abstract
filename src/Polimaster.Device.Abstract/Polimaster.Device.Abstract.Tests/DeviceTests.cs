using System.IO;
using System.Threading;
using Moq;
using Polimaster.Device.Abstract.Tests.Device;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests;

public class DeviceTests {
    private readonly Mock<ITransport<string, string>> _transportMock;

    public DeviceTests() {
        _transportMock = new Mock<ITransport<string, string>>();
        var stream = new Mock<Stream>();
        _transportMock.Setup(x => x.Open()).ReturnsAsync(stream.Object);
    }

    [Fact]
    public async void ShouldWrite() {
        var dev = new MyDevice(_transportMock.Object);

        var myCommand = new MyCommand {
            Param = new MyParam { CommandPid = 0, Value = "write test" }
        };
        await dev.Write(myCommand);

        _transportMock.Verify(v => v.Write(It.IsAny<Stream>(), myCommand.Compile(), CancellationToken.None));
    }

    [Fact]
    public async void ShouldRead() {
        var dev = new MyDevice(_transportMock.Object);

        var myCommand = new MyReadCommand {
            Param = new MyParam { CommandPid = 0, Value = "read test" }
        };
        await dev.Read(myCommand);

        _transportMock.Verify(v => v.Read(It.IsAny<Stream>(), myCommand.Compile(), CancellationToken.None));
    }
}