using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Impl;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands; 

public class TemperatureReader(ITransport<string> transport, ILoggerFactory? loggerFactory)
    : StringReader<double?>(transport, loggerFactory) {
    protected override string Compile() => $"{Cmd.PREFIX}{Cmd.QUESTION_MARK}TEMPERATURE";

    protected override double? Parse(string? res) {
        return 22;
    }
}