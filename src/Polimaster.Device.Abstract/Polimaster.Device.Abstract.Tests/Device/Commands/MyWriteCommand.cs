using Polimaster.Device.Abstract.Device.Commands;

namespace Polimaster.Device.Abstract.Tests.Device.Commands;

public class MyWriteCommand : AWriteCommand<MyParam, string> {

    public string CompiledCommand => Compile();

    protected override string Compile() {
        return $"{Value?.CommandPid} : {Value?.Value}";
    }
    
}