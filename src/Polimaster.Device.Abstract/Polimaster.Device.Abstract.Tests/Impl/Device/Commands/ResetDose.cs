using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands; 

public class ResetDose : ACommand<string> {
    public ResetDose(ILoggerFactory? loggerFactory) : base(loggerFactory) {
    }

    protected override string Compile() => $"{Cmd.PREFIX}{Cmd.QUESTION_MARK}DOSE_RESET";
}