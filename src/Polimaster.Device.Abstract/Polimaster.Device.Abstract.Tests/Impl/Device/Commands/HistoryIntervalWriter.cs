﻿using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Impl;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands; 

public class HistoryIntervalWriter : StringWriter<ushort> {
    public HistoryIntervalWriter(ILoggerFactory? loggerFactory) : base(loggerFactory) {
    }

    protected override string Compile(ushort data) {
        return $"{Cmd.PREFIX}INTERVAL:{data}";
    }
}