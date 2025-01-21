using System.Text;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Tests.Impl.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Commands; 

public class HistoryWiper(IMyTransport transport, ILoggerFactory? loggerFactory)
    : MyDeviceStreamCommand(transport, loggerFactory) {

    public static byte[] COMMAND = Encoding.UTF8.GetBytes($"{Cmd.PREFIX}{Cmd.QUESTION_MARK}HISTORY_WIPE");

    protected override byte[] Compile() => COMMAND;
}