using Polimaster.Device.Abstract.Commands;

namespace Polimaster.Device.Abstract.Tests.Device;

public class MyResultCommand : AReadCommand<string, string> {
    
    public MyParam? Param { get; init; }
    
    protected override string Parse(string result) {
        return result;
    }
    protected override string Compile() {
        return $"{Param?.CommandPid} : {Param?.Value}";
    }
}