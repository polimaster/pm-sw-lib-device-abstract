using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Tests.Impl.Device.Settings;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands; 

public class MyReadCommand : StringCommandRead<MyParam> {
    public MyReadCommand(ILoggerFactory? loggerFactory = null) : base(loggerFactory) {
    }

    protected override string Compile() {
        return $"{Value?.CommandPid} : {Value?.Value}";
    }

    protected override MyParam? Parse(string? data) {
        Value ??= new MyParam();
        Value.Value = data;
        return Value;
    }
}