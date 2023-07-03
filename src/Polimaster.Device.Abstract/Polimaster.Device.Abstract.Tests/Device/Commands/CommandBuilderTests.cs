using Polimaster.Device.Abstract.Device.Commands;

namespace Polimaster.Device.Abstract.Tests.Device.Commands; 

public class CommandBuilderTests : Mocks {
    
    private readonly CommandBuilder<string> _builder = new (LOGGER_FACTORY);

    [Fact]
    public void ShouldBuildCommand() {
        var deviceMock = DeviceMock;
        var transportMock = TransportMock;
        deviceMock.Setup(x => x.Transport).Returns(transportMock.Object);
        
        var command = _builder.Build<MyWriteCommand, MyParam>(deviceMock.Object);
        
        Assert.Equal(transportMock.Object, command.Transport);
        Assert.NotNull(command.Logger);
    }

    [Fact]
    public void ShouldCacheIdenticalCommands() {
        var deviceMock = DeviceMock;
        deviceMock.Setup(x => x.Id).Returns("DEVICE_ID");
        
        var command1 = _builder.Build<MyWriteCommand, MyParam>(deviceMock.Object);
        var command2 = _builder.Build<MyWriteCommand, MyParam>(deviceMock.Object);
        
        Assert.Equal(command1, command2);
    }
    
    
}