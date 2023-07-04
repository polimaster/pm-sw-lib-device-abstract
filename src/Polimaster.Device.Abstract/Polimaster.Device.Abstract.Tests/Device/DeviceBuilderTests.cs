using System;
using Polimaster.Device.Abstract.Device;

namespace Polimaster.Device.Abstract.Tests.Device; 

public class DeviceBuilderTests : Mocks {

    [Fact]
    public void ShouldBuildDeviceAndTrackTransport() {

        var commandBuilderMock = CommandBuilderMock;
        var settingBuilderMock = SettingBuilderMock;
        var transportMock = TransportMock;

        var builder = new DeviceBuilder<string>(commandBuilderMock.Object, settingBuilderMock.Object, LOGGER_FACTORY);

        var device = builder.With(transportMock.Object).Build<MyDevice>();
        
        Assert.Equal(commandBuilderMock.Object, device.CommandBuilder);
        Assert.Equal(settingBuilderMock.Object, device.SettingBuilder);
        Assert.Equal(transportMock.Object, device.Transport);
        
        
        transportMock = TransportMock;
        
        var device1 = builder.With(transportMock.Object).Build<MyDevice>();
        
        Assert.NotEqual(device1.Transport, device.Transport);
    }


    [Fact]
    public void ShouldNotBuildWithoutTransport() {
        
        var commandBuilderMock = CommandBuilderMock;
        var settingBuilderMock = SettingBuilderMock;

        var builder = new DeviceBuilder<string>(commandBuilderMock.Object, settingBuilderMock.Object, LOGGER_FACTORY);
        
        MyDevice? device = null;

        try {
            device = builder.Build<MyDevice>();    
        } catch (DeviceException e) {
            
        }
        
        Assert.Null(device);
        
    }
}