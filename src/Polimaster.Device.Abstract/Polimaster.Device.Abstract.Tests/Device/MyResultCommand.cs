using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Commands;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Device;

public class MyResultCommand : AResultCommand<string, string> {
    
    public MyParam? Param { get; set; }
    
    protected override string Parse(string result) {
        return result;
    }
    protected override string Compile() {
        return $"{Param?.CommandPid} : {Param?.Value}";
    }
}