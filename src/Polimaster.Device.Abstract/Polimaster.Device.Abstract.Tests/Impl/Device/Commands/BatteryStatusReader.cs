using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Impl;
using Polimaster.Device.Abstract.Device.Implementations;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands;

public class BatteryStatusReader(ITransport<string> transport, ILoggerFactory? loggerFactory)
    : StringReader<BatteryStatus?>(transport, loggerFactory) {
    protected override string Compile() => $"{Cmd.PREFIX}{Cmd.QUESTION_MARK}BAT";

    protected override BatteryStatus? Parse(string? res) {
        return new BatteryStatus {
            Volts = 0.8,
            Percents = 88
        };
    }
}