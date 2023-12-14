using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Impl;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands; 

/// <summary>
/// Shutdown device
/// </summary>
public class SwitchOff : StringCommand {
    private const string COMMAND = "SWITCH_OFF";
    public string Compiled => Compile();
    
    public SwitchOff(ILoggerFactory? loggerFactory) : base(loggerFactory) {
    }

    protected override string Compile() {
        return $"{Cmd.PREFIX}{COMMAND}";
    }
}