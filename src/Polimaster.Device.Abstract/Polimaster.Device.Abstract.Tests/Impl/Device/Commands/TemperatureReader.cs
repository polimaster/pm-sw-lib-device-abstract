using System.Text;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands; 

public class TemperatureReader(ITransport transport, ILoggerFactory? loggerFactory)
    : ADataReader<double?>(transport, loggerFactory) {
    protected override byte[] Compile() => Encoding.UTF8.GetBytes($"{Cmd.PREFIX}{Cmd.QUESTION_MARK}TEMPERATURE");

    protected override double? Parse(byte[] res) {
        return 22;
    }
}