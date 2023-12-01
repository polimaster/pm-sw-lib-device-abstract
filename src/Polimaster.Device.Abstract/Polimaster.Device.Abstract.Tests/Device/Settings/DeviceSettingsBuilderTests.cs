using Polimaster.Device.Abstract.Device.Settings;

namespace Polimaster.Device.Abstract.Tests.Device.Settings; 

public class DeviceSettingsBuilderTests : Mocks {

    [Fact]
    public void ShouldBuildSettingAndTrackCommandInstances() {
        var builder = new SettingBuilder();

        var readCommandMock = CommandMock;
        var writeCommandMock = CommandMock;

        var setting = builder.WithReadCommand(readCommandMock.Object)
            .WithWriteCommand(writeCommandMock.Object).Build<MyParam>();
        
        Assert.Equal(readCommandMock.Object, setting.ReadCommand);
        Assert.Equal(writeCommandMock.Object, setting.WriteCommand);
        
        readCommandMock = CommandMock;
        writeCommandMock = CommandMock;
        
        var setting1 = builder.WithReadCommand(readCommandMock.Object)
            .WithWriteCommand(writeCommandMock.Object).Build<MyParam>();
        
        Assert.NotEqual(setting.ReadCommand, setting1.ReadCommand);
        Assert.NotEqual(setting.WriteCommand, setting1.WriteCommand);
        Assert.NotEqual(setting, setting1);
    }

    [Fact]
    public void ShouldCreateTargetInstance() {
        var builder = new SettingBuilder();
        
        var setting = builder.WithImplementation<MyDeviceSetting, MyParam>().Build<MyParam>();
        
        Assert.True(setting.GetType() == typeof(MyDeviceSetting));
    }

    // [Fact]
    // public void ShouldCreateProxy() {
    //     var builder = new SettingBuilder();
    //
    //     var setting = builder.BuildWithProxy<MyDeviceSettingProxy, string, string>(TODO);
    //     
    //     Assert.True(setting.GetType() == typeof(MyDeviceSettingProxy));
    // }
}