using System.Text;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Tests.Impl.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Commands; 

public class PlainWriter(IMyTransport transport, ILoggerFactory? loggerFactory) : MyDeviceStreamWriter<string>(transport, loggerFactory) {
    protected override byte[] Compile(string? data) => Encoding.UTF8.GetBytes($"{Cmd.PREFIX}{data}");
}