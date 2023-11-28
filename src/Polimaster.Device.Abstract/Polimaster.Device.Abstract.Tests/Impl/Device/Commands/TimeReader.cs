using System;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Impl;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands; 

public class TimeReader : StringReader<DateTime> {
    public TimeReader(ILoggerFactory? loggerFactory) : base(loggerFactory) {
    }

    protected override string Compile() {
        return $"{Cmd.PREFIX}{Cmd.QUESTION_MARK}TIME";
    }

    protected override DateTime Parse(string res) {
        return DateTime.Now;
    }
}