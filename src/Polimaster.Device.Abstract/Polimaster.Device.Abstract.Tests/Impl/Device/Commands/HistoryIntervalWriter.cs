using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Impl;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands; 

public class HistoryIntervalWriter(ITransport<string> transport, ILoggerFactory? loggerFactory) : StringWriter<ushort?>(transport, loggerFactory) {
    protected override string Compile(ushort? data) {
        return $"{Cmd.PREFIX}INTERVAL:{data}";
    }
}