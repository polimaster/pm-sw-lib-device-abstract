using System.Threading;
using Moq;
using Polimaster.Device.Abstract.Commands;
using Polimaster.Device.Abstract.Device;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests;

public class DeviceTests {
    
    public class MyDevice : ADevice<string> {
        public MyDevice(ITransport<string> transport) : base(transport) {
        }
    }

    public class MyCommand : ICommand<string, string> {
        public string Param { get; set; }
        public string Compile() {
            return Param;
        }
    }

    public class MyReadCommand : MyCommand, IReadCommand<string, string, string> {
        public string? Result { get; private set; }
        public string? Parse(string result) {
            Result = result;
            return result;
        }
    }
    
    private readonly Mock<ITransport<string>> _transportMock;
    
    public DeviceTests() {
        _transportMock = new Mock<ITransport<string>>();
    }

    [Fact]
    public async void ShouldWrite() {
        var dev = new MyDevice(_transportMock.Object);

        var myCommand = new MyCommand {
            Param = "test"
        };
        await dev.Write(myCommand);
        
        _transportMock.Verify(v => v.Write(myCommand.Compile(), CancellationToken.None));
    }

    [Fact]
    public async void ShouldRead() {
        var dev = new MyDevice(_transportMock.Object);

        var myCommand = new MyReadCommand {
            Param = "test"
        };
        await dev.Read(myCommand);
        
        _transportMock.Verify(v => v.Read(myCommand.Compile(), CancellationToken.None));
    }
}