using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Tests.Impl.Device.Settings;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands;

public class MyWriteCommand : StringCommandWrite<MyParam> {
    public MyWriteCommand(ILoggerFactory? loggerFactory = null) : base(loggerFactory) {
    }

    protected override string Compile(object value) {
        return $"{Value?.CommandPid} : {Value?.Value}";
    }
}