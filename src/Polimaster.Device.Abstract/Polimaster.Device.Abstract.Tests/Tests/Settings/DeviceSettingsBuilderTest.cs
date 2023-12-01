// using System;
// using Moq;
// using Polimaster.Device.Abstract.Device.Commands;
// using Polimaster.Device.Abstract.Device.Settings;
// using Polimaster.Device.Abstract.Tests.Impl.Device.Settings;
//
// namespace Polimaster.Device.Abstract.Tests.Tests.Settings; 
//
// public class DeviceSettingsBuilderTest : Mocks {
//     private readonly SettingBuilder _builder = new();
//     private readonly Mock<IDataReader<MyParam>> _readerMock = new();
//     private readonly Mock<IDataWriter<MyParam>> _writerMock = new();
//
//     [Fact]
//     public void ShouldBuildSetting() {
//         var setting = _builder.WithReader(_readerMock.Object)
//             .WithWriter(_writerMock.Object).Build<MyParam>();
//         
//         Assert.IsType<DeviceSettingBase<MyParam>>(setting);
//     }
//
//     [Fact]
//     public void SettingShouldBeReadOnly() {
//         var s1 = _builder.WithReader(_readerMock.Object).Build<MyParam>();
//         Assert.True(s1.ReadOnly);
//     }
//
//     [Fact]
//     public void ShouldFailIfNoReader() {
//         Exception? exception = null;
//         try {
//             _builder.Build<MyParam>();
//         } catch (Exception e) {
//             exception = e;
//         }
//         Assert.NotNull(exception);
//         Assert.IsType<NullReferenceException>(exception);
//     }
//
//     [Fact]
//     public void ShouldFailOnTypesMismatch() {
//         Exception? e1 = null;
//         var reader = new Mock<IDataReader<int>>();
//         try {
//             _builder.WithReader(reader.Object).Build<MyParam>();
//         } catch (Exception e) {
//             e1 = e;
//         }
//         Assert.NotNull(e1);
//         Assert.IsType<ArgumentException>(e1);
//         
//         Exception? e2 = null;
//         var writer = new Mock<IDataWriter<int>>();
//         try {
//             _builder.WithReader(_readerMock.Object).WithWriter(writer.Object).Build<MyParam>();
//         } catch (Exception e) {
//             e2 = e;
//         }
//         Assert.NotNull(e2);
//         Assert.IsType<ArgumentException>(e2);
//     }
//
//     [Fact]
//     public void ShouldBuildImplementation() {
//         var setting = _builder.WithReader(_readerMock.Object)
//             .WithWriter(_writerMock.Object).WithImplementation<MyParamSetting, MyParam>().Build<MyParam>();
//         
//         Assert.IsType<MyParamSetting>(setting);
//     }
//
//     [Fact]
//     public void ShouldBuildProxy() {
//         var setting = _builder.WithReader(_readerMock.Object).WithWriter(_writerMock.Object).Build<MyParam>();
//         var proxy = _builder.BuildWithProxy<MyParamSettingProxy, string, MyParam>(setting);
//         
//         Assert.IsType<MyParamSettingProxy>(proxy);
//     }
// }