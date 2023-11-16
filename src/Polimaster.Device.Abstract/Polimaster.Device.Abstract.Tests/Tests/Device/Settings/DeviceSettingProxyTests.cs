using System.Threading;
using Moq;
using Polimaster.Device.Abstract.Tests.Impl.Device.Settings;

namespace Polimaster.Device.Abstract.Tests.Tests.Device.Settings; 

public class DeviceSettingProxyTests : Mocks {

    [Fact]
    public void ShouldReadWriteProxiedSetting() {

        const string value = "SETTING_VALUE";
        
        var settingMock = SettingMock;
        settingMock.Setup(x => x.Value).Returns(value);

        var proxy = new MyDeviceSettingProxy {
            ProxiedSetting = settingMock.Object
        };
        
        Assert.Equal(value, proxy.Value);

        var newProxyValue = "newProxyValue";
        proxy.Value = newProxyValue;
        
        settingMock.VerifySet(x => x.Value = newProxyValue);

    }

    [Fact]
    public async void ShouldCallReadOfProxied() {
        var settingMock = SettingMock;

        var proxy = new MyDeviceSettingProxy {
            ProxiedSetting = settingMock.Object
        };

        await proxy.Read(CancellationToken.None);
        
        settingMock.Verify(x => x.Read(It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async void ShouldCallCommitOfProxied() {
        var settingMock = SettingMock;

        var proxy = new MyDeviceSettingProxy {
            ProxiedSetting = settingMock.Object,
            Value = "NEW_VALUE"
        };

        await proxy.CommitChanges(CancellationToken.None);
        
        settingMock.Verify(x => x.CommitChanges(It.IsAny<CancellationToken>()));
    }
    
}