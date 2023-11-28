using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Impl;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands; 

public class PlainWriter : StringWriter<string> {
    public PlainWriter(ILoggerFactory? loggerFactory) : base(loggerFactory) {
    }

    protected override string Compile(string? data) => $"{Cmd.PREFIX}{data}";
}