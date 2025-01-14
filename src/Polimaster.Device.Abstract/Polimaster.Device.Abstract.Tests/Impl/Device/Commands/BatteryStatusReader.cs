using System.Text;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Device.Implementations;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands;

public class BatteryStatusReader(ITransport transport, ILoggerFactory? loggerFactory)
    : ADataReader<BatteryStatus?>(transport, loggerFactory) {
    protected override byte[] Compile() => Encoding.UTF8.GetBytes($"{Cmd.PREFIX}{Cmd.QUESTION_MARK}BAT");

    protected override BatteryStatus? Parse(byte[] res) {
        return new BatteryStatus {
            Volts = 0.8,
            Percents = 88
        };
    }
}