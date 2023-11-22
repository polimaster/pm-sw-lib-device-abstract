// using Polimaster.Device.Abstract.Device.Commands;
// using Polimaster.Device.Abstract.Tests.Impl.Device;
// using Polimaster.Device.Abstract.Tests.Impl.Device.Commands;
// using Polimaster.Device.Abstract.Tests.Impl.Device.Settings;
//
// namespace Polimaster.Device.Abstract.Tests.Tests.Device.Commands; 
//
// public class CommandBuilderTests : Mocks {
//     
//     private readonly CommandBuilder _builder = new (LOGGER_FACTORY);
//
//     [Fact]
//     public void ShouldBuildCommand() {
//         var deviceMock = DeviceMock;
//         var transportMock = TransportMock;
//         deviceMock.Setup(x => x.Transport).Returns(transportMock.Object);
//
//         _builder.With(deviceMock.Object);
//         var command = _builder.Build<MyParamWriter, MyParam>();
//         
//         Assert.Equal(transportMock.Object, command.Device.Transport);
//         Assert.NotNull(command.Logger);
//     }
//
//     [Fact]
//     public void ShouldBuildNewCommandWhenDeviceIsDisposed() {
//         var transportMock = TransportMock;
//         transportMock.Setup(x => x.ConnectionId).Returns("CONNECTION_ID");
//
//         var device = new MyDevice(transportMock.Object);
//
//         _builder.With(device);
//         var command1 = _builder.Build<MyParamWriter, MyParam>();
//         
//         device.Dispose();
//         
//         var command2 = _builder.Build<MyParamWriter, MyParam>();
//         
//         Assert.NotEqual(command1, command2);
//     }
// }