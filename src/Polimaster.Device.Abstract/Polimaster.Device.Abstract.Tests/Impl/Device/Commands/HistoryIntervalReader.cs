using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Impl;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands; 

public class HistoryIntervalReader(ILoggerFactory? loggerFactory) : StringReader<ushort?>(loggerFactory) {
    protected override string Compile() => $"{Cmd.PREFIX}{Cmd.QUESTION_MARK}INTERVAL";

    protected override ushort? Parse(string? res) {
        return 1;
    }
}