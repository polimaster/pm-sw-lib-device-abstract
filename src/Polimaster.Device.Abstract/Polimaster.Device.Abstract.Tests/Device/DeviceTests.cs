using System.Linq;
using System.Threading;

namespace Polimaster.Device.Abstract.Tests.Device;

public class DeviceTests : Mocks {

    [Fact]
    public void ShouldReturnProperties() {

        var device = new MyDevice();
        var settings = device.GetDeviceSettingsProperties();
        
        Assert.True(settings.Any());
    }

    [Fact]
    public async void ShouldCallReadOnSetting() {
        
        var settingMock = SettingMock;
        var device = new MyDevice {
            TestSetting = settingMock.Object
        };

        var token = new CancellationToken();
        await device.ReadSettings(token);
        
        settingMock.Verify(x => x.Read(token));

    }
    
    [Fact]
    public async void ShouldCallWriteOnSetting() {
        
        var settingMock = SettingMock;
        var device = new MyDevice {
            TestSetting = settingMock.Object
        };

        var token = new CancellationToken();
        await device.WriteSettings(token);
        
        settingMock.Verify(x => x.CommitChanges(token));

    }
}