using System.Text;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Tests.Impl.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Commands; 

public class HistoryIntervalWriter(IMyTransport transport, ILoggerFactory? loggerFactory) : MyDeviceStreamWriter<ushort>(transport, loggerFactory) {
    protected override byte[] Compile(ushort data) {
        return Encoding.UTF8.GetBytes($"{Cmd.PREFIX}INTERVAL:{data}");
    }
}