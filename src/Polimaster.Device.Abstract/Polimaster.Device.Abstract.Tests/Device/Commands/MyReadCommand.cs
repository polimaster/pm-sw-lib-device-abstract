using Polimaster.Device.Abstract.Device.Commands;

namespace Polimaster.Device.Abstract.Tests.Device.Commands; 

public class MyReadCommand : AReadCommand<MyParam?, string> {
    protected override string Compile() {
        return $"{Value?.CommandPid} : {Value?.Value}";
    }

    protected override MyParam? Parse(string data) {
        if (Value != null) Value.Value = data;
        return Value;
    }
}