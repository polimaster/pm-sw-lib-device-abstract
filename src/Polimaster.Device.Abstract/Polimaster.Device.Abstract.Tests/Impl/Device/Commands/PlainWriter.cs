using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Impl;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands; 

public class PlainWriter(ITransport<string> transport, ILoggerFactory? loggerFactory) : StringWriter<string?>(transport, loggerFactory) {
    protected override string Compile(string? data) => $"{Cmd.PREFIX}{data}";
}