using System.Text;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Tests.Impl.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Commands; 

/// <summary>
/// Shutdown device
/// </summary>
public class SwitchOff(IMyTransport transport, ILoggerFactory? loggerFactory)
    : MyDeviceStreamCommand(transport, loggerFactory) {
    private const string COMMAND = "SWITCH_OFF";

    protected override byte[] Compile() => Encoding.UTF8.GetBytes($"{Cmd.PREFIX}{COMMAND}");
}