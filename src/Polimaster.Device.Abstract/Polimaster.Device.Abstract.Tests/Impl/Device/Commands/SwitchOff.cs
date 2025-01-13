using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Impl;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands; 

/// <summary>
/// Shutdown device
/// </summary>
public class SwitchOff(ITransport<string> transport, ILoggerFactory? loggerFactory)
    : StringCommand(transport, loggerFactory) {
    private const string COMMAND = "SWITCH_OFF";
    public string Compiled => Compile();

    protected override string Compile() {
        return $"{Cmd.PREFIX}{COMMAND}";
    }
}