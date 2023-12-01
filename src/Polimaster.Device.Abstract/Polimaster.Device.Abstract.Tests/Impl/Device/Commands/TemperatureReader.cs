using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Impl;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands; 

public class TemperatureReader : StringReader<double?> {
    public TemperatureReader(ILoggerFactory? loggerFactory) : base(loggerFactory) {
    }

    protected override string Compile() => $"{Cmd.PREFIX}{Cmd.QUESTION_MARK}TEMPERATURE";

    protected override double? Parse(string? res) {
        return 22;
    }
}