using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Impl;
using Polimaster.Device.Abstract.Tests.Impl.Device.Settings;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands;

public class MyParamWriter : StringWriter<MyParam> {
    public MyParamWriter(ILoggerFactory? loggerFactory = null) : base(loggerFactory) {
    }

    protected override string Compile(MyParam? data) {
        return $"{Cmd.PREFIX}{data?.CommandPid}:{data?.Value}";
    }
}