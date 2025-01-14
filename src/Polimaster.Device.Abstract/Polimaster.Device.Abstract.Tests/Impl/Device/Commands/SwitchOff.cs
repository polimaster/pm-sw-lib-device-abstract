using System.Text;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands; 

/// <summary>
/// Shutdown device
/// </summary>
public class SwitchOff(ITransport transport, ILoggerFactory? loggerFactory)
    : ACommand(transport, loggerFactory) {
    private const string COMMAND = "SWITCH_OFF";

    protected override byte[] Compile() => Encoding.UTF8.GetBytes($"{Cmd.PREFIX}{COMMAND}");
}