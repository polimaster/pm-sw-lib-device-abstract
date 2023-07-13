using Polimaster.Device.Abstract.Device;
using Polimaster.Device.Abstract.Device.Commands;

namespace Polimaster.Device.Abstract.Tests.Device; 

public class DeviceBuilderTests : Mocks {

    [Fact]
    public void ShouldBuildDeviceAndTrackTransport() {
        
        var settingBuilderMock = SettingBuilderMock;
        var transportMock = TransportMock;
        
        var commandBuilder = new CommandBuilder(LOGGER_FACTORY);
        var builder = new DeviceBuilder(commandBuilder, settingBuilderMock.Object, LOGGER_FACTORY);

        var device = builder.With(transportMock.Object).Build<MyDevice>();

        Assert.NotNull(device.CommandBuilder);
        Assert.Equal(settingBuilderMock.Object, device.SettingBuilder);
        Assert.Equal(transportMock.Object, device.Transport);
        
        transportMock = TransportMock;
        
        var device1 = builder.With(transportMock.Object).Build<MyDevice>();
        
        Assert.NotEqual(device1.Transport, device.Transport);
    }


    [Fact]
    public void ShouldNotBuildWithoutTransport() {
        
        var settingBuilderMock = SettingBuilderMock;

        var commandBuilder = new CommandBuilder(LOGGER_FACTORY);
        var builder = new DeviceBuilder(commandBuilder, settingBuilderMock.Object, LOGGER_FACTORY);
        
        MyDevice? device = null;

        try {
            device = builder.Build<MyDevice>();    
        } catch (DeviceException) {
            
        }
        
        Assert.Null(device);
        
    }
}