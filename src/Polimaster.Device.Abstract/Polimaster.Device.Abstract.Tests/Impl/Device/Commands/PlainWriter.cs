using System.Text;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Impl;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands; 

public class PlainWriter(ITransport transport, ILoggerFactory? loggerFactory) : StringWriter(transport, loggerFactory) {
    protected override byte[] Compile(string? data) => Encoding.UTF8.GetBytes($"{Cmd.PREFIX}{data}");
}