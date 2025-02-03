using System.Text;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Implementations;
using Polimaster.Device.Abstract.Tests.Impl.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Commands;

public class BatteryStatusReader(IMyTransport transport, ILoggerFactory? loggerFactory)
    : MyDeviceStreamReader<BatteryStatus>(transport, loggerFactory) {
    protected override byte[] Compile() => Encoding.UTF8.GetBytes($"{Cmd.PREFIX}{Cmd.QUESTION_MARK}BAT");

    protected override BatteryStatus Parse(byte[]? res) {
        return new BatteryStatus {
            Volts = 0.8,
            Percents = 88
        };
    }
}