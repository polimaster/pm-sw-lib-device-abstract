using System;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Impl;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands; 

public class TimeWriter : StringWriter<DateTime> {
    public TimeWriter(ILoggerFactory? loggerFactory) : base(loggerFactory) {
    }

    protected override string Compile(DateTime data) {
        return $"{Cmd.PREFIX}TIME:{data}";
    }
}