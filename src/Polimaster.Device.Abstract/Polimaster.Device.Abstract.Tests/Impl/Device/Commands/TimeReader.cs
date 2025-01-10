using System;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Impl;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands; 

public class TimeReader(ITransport<string> transport, ILoggerFactory? loggerFactory)
    : StringReader<DateTime>(transport, loggerFactory) {
    protected override string Compile() => $"{Cmd.PREFIX}{Cmd.QUESTION_MARK}TIME";

    protected override DateTime Parse(string? res) {
        return DateTime.Now;
    }
}