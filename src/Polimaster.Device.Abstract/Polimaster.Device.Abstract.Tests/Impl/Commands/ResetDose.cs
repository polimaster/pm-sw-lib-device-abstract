using System.Text;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Tests.Impl.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Commands;

public class ResetDose(IMyTransport transport, ILoggerFactory? loggerFactory)
    : MyDeviceStreamCommand(transport, loggerFactory) {
    protected override byte[] Compile() => Encoding.UTF8.GetBytes($"{Cmd.PREFIX}{Cmd.QUESTION_MARK}DOSE_RESET");
}