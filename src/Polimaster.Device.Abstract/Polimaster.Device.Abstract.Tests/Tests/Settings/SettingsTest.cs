using Moq;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Helpers;
using Polimaster.Device.Abstract.Tests.Impl.Settings;
using Polimaster.Device.Abstract.Tests.Impl.Transport;

namespace Polimaster.Device.Abstract.Tests.Tests.Settings;

public class BoolSettingTest: Mocks {

    [Fact]
    public void BoolSettingShouldReturnCorrectStringValue() {
        var transport = new Mock<IMyTransport>();
        var setting = new BoolSetting(transport.Object, SETTING_DESCRIPTORS, LOGGER_FACTORY) {
            Value = true
        };

        Assert.Equal(OnOff.ON.GetDescription(), setting.ToString());

        setting.Value = false;

        Assert.Equal(OnOff.OFF.GetDescription(), setting.ToString());
    }



    [Fact]
    public void EnumSettingShouldReturnCorrectStringValue() {
        var transport = new Mock<IMyTransport>();
        var setting = new OnOffSetting(transport.Object, SETTING_DESCRIPTORS, LOGGER_FACTORY) {
            Value = OnOff.ON
        };

        Assert.Equal(OnOff.ON.GetDescription(), setting.ToString());

        setting.Value = OnOff.OFF;

        Assert.Equal(OnOff.OFF.GetDescription(), setting.ToString());
    }
}