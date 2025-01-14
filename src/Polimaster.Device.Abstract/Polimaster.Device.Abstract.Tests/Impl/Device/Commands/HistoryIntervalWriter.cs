using System.Text;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands; 

public class HistoryIntervalWriter(ITransport transport, ILoggerFactory? loggerFactory) : ADataWriter<ushort?>(transport, loggerFactory) {
    protected override byte[] Compile(ushort? data) {
        return Encoding.UTF8.GetBytes($"{Cmd.PREFIX}INTERVAL:{data}");
    }
}