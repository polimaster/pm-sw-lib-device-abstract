using System;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Impl;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands; 

public class TimeWriter : StringWriter<DateTime> {
    public TimeWriter(ITransport<string> transport, ILoggerFactory? loggerFactory) : base(transport, loggerFactory) {
    }

    protected override string Compile(DateTime data) {
        return $"{Cmd.PREFIX}TIME:{data}";
    }
}