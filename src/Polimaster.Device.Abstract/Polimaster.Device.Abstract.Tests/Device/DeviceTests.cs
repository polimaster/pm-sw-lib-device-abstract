// using System.IO;
// using System.Threading;
// using Moq;
// using Polimaster.Device.Abstract.Tests.Device.Commands;
// using Polimaster.Device.Abstract.Transport.Interfaces;
//
// namespace Polimaster.Device.Abstract.Tests.Device;
//
// public class DeviceTests {
//     private readonly Mock<ITransport<string>> _transportMock;
//
//     public DeviceTests() {
//         _transportMock = new Mock<ITransport<string>>();
//         var stream = new Mock<Stream>();
//         _transportMock.Setup(x => x.Open()).ReturnsAsync(stream.Object);
//     }
//
//     [Fact]
//     public async void ShouldWrite() {
//         var myCommand = new MyWriteCommand {
//             Param = new MyParam { CommandPid = 0, Value = "write test" },
//             Transport = _transportMock.Object
//         };
//         
//         // simulate compilation
//         var compiled = $"{myCommand.Param?.CommandPid} : {myCommand.Param?.Value}";
//
//         await myCommand.Send();
//
//         _transportMock.Verify(v => v.Write(It.IsAny<Stream>(), compiled, CancellationToken.None));
//     }
//
//     [Fact]
//     public async void ShouldRead() {
//
//         var myCommand = new MyResultCommand {
//             Param = new MyParam { CommandPid = 0, Value = "read test" },
//             Transport = _transportMock.Object
//         };
//         
//         // simulate compilation
//         var compiled = $"{myCommand.Param?.CommandPid} : {myCommand.Param?.Value}";
//
//         _transportMock.Setup(x => x.Read(It.IsAny<Stream>(), compiled, CancellationToken.None)).ReturnsAsync(compiled);
//         
//         await myCommand.Send();
//         var res = myCommand.Value;
//
//         _transportMock.Verify(v => v.Read(It.IsAny<Stream>(), compiled, CancellationToken.None));
//         Assert.Equal(compiled, res);
//     }
// }