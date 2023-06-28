using Polimaster.Device.Abstract.Commands;

namespace Polimaster.Device.Abstract.Tests.Device;

public class MyCommand : AWriteCommand<string, string> {
    public MyParam? Param { get; set; }

    protected override string Compile() {
        return $"{Param?.CommandPid} : {Param?.Value}";
    }
    
}