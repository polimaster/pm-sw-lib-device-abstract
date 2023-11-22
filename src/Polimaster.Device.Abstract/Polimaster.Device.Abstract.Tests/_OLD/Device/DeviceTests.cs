// using System.Linq;
// using System.Threading;
// using Polimaster.Device.Abstract.Tests.Impl.Device;
//
// namespace Polimaster.Device.Abstract.Tests.Tests.Device;
//
// public class DeviceTests : Mocks {
//
//     [Fact]
//     public void ShouldReturnProperties() {
//
//         var device = new MyDevice();
//         var settings = device.GetDeviceSettingsProperties();
//         
//         Assert.True(settings.Any());
//     }
//
//     [Fact]
//     public async void ShouldCallReadOnSetting() {
//         
//         var settingMock = SettingMock;
//         var device = new MyDevice {
//             MyParamSetting = settingMock.Object
//         };
//
//         var token = new CancellationToken();
//         await device.ReadAllSettings(token);
//         
//         settingMock.Verify(x => x.Read(token));
//
//     }
//     
//     [Fact]
//     public async void ShouldCallWriteOnSetting() {
//         
//         var settingMock = SettingMock;
//         var device = new MyDevice {
//             MyParamSetting = settingMock.Object
//         };
//
//         var token = new CancellationToken();
//         await device.WriteAllSettings(token);
//         
//         settingMock.Verify(x => x.CommitChanges(token));
//
//     }
// }