using System.Text;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Tests.Impl.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Commands; 

public class TemperatureReader(IMyTransport transport, ILoggerFactory? loggerFactory)
    : MyDeviceStreamReader<double>(transport, loggerFactory) {
    protected override byte[] Compile() => Encoding.UTF8.GetBytes($"{Cmd.PREFIX}{Cmd.QUESTION_MARK}TEMPERATURE");

    protected override double Parse(byte[]? res) {
        return 22;
    }
}