using Polimaster.Device.Abstract.Device.Commands;

namespace Polimaster.Device.Abstract.Tests.Device.Commands; 

public class CommandBuilderTests : Mocks {
    
    private readonly CommandBuilder<string> _builder = new (LOGGER_FACTORY);

    [Fact]
    public void ShouldBuildCommand() {
        var deviceMock = DeviceMock;
        var transportMock = TransportMock;
        deviceMock.Setup(x => x.Transport).Returns(transportMock.Object);

        _builder.Device = deviceMock.Object;
        var command = _builder.Build<MyWriteCommand, MyParam>();
        
        Assert.Equal(transportMock.Object, command.Device.Transport);
        Assert.NotNull(command.Logger);
    }

    [Fact]
    public void ShouldBuildNewCommandWhenDeviceIsDisposed() {
        var transportMock = TransportMock;
        transportMock.Setup(x => x.ConnectionId).Returns("CONNECTION_ID");
        var device = new MyDevice {
            Transport = transportMock.Object,
        };

        _builder.Device = device;
        var command1 = _builder.Build<MyWriteCommand, MyParam>();
        
        device.Dispose();
        
        var command2 = _builder.Build<MyWriteCommand, MyParam>();
        
        Assert.NotEqual(command1, command2);
    }
}