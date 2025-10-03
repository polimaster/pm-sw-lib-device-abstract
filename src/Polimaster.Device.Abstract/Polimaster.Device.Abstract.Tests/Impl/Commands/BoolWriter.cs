using System.Text;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Tests.Impl.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Commands;

public class BoolWriter(IMyTransport transport, ILoggerFactory? loggerFactory) : MyDeviceStreamWriter<bool>(transport, loggerFactory) {
    protected override byte[] Compile(bool data) {
        return Encoding.UTF8.GetBytes($"{Cmd.PREFIX}BOOL:{data}");
    }
}