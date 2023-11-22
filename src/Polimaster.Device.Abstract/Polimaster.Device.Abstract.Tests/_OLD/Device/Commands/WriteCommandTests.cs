// using System;
// using Moq;
// using Polimaster.Device.Abstract.Tests.Impl.Device;
// using Polimaster.Device.Abstract.Tests.Impl.Device.Commands;
// using Polimaster.Device.Abstract.Tests.Impl.Device.Settings;
//
// namespace Polimaster.Device.Abstract.Tests.Tests.Device.Commands; 
//
// public class WriteCommandTests : Mocks {
//
//     [Fact]
//     public async void ShouldThrowExceptionIfTransportNUll() {
//
//         var command = new MyParamWriter();
//         
//         await Assert.ThrowsAsync<NullReferenceException>(() => command.Send());
//     }
//
//     [Fact]
//     public async void ShouldCallValueChanged() {
//         var writerMock = DeviceStreamMock;
//
//         var transportMock = TransportMock;
//         transportMock.Setup(x => x.OpenAsync()).ReturnsAsync(writerMock.Object);
//
//         var command = new MyParamWriter {
//             Device = new MyDevice(transportMock.Object),
//             Value = new MyParam { CommandPid = 1, Value = "test"}
//         };
//
//         MyParam? expected = null;
//         command.ValueChanged += s => expected = s;
//
//         await command.Send();
//         
//         Assert.Equal(expected, command.Value);
//         
//     }
//
// }