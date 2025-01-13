using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Impl;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands; 

public class HistoryIntervalReader(ITransport<string> transport, ILoggerFactory? loggerFactory) : StringReader<ushort?>(transport, loggerFactory) {
    protected override string Compile() => $"{Cmd.PREFIX}{Cmd.QUESTION_MARK}INTERVAL";

    protected override ushort? Parse(string? res) {
        return 1;
    }
}