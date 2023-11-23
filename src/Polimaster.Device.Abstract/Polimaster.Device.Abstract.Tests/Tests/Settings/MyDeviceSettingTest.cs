using Moq;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Tests.Impl.Device.Settings;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Tests.Settings; 

public class MyDeviceSettingTest : Mocks {
    private readonly Mock<ITransport> _transport = new();
    private readonly Mock<IDataReader<MyParam>> _reader = new();
    private readonly Mock<IDataWriter<MyParam>> _dataWriter = new();

    [Fact]
    public void ShouldValidateValue() {
        var setting = new MyDeviceSetting(_transport.Object, _reader.Object) {
            Value = null
        };
        Assert.True(setting.IsDirty);
        Assert.False(setting.IsValid);

        setting = new MyDeviceSetting(_transport.Object, _reader.Object) {
            Value = new MyParam { Value = "Very long string that does not pass validation" }
        };
        Assert.True(setting.IsDirty);
        Assert.False(setting.IsValid);
        
        setting = new MyDeviceSetting(_transport.Object, _reader.Object) {
            Value = new MyParam { Value = "Valid" }
        };
        Assert.True(setting.IsDirty);
        Assert.True(setting.IsValid);
        
    }
}