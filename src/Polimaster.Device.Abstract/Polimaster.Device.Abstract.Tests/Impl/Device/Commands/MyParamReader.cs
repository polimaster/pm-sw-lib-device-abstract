using System;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Exceptions;
using Polimaster.Device.Abstract.Device.Commands.Impl;
using Polimaster.Device.Abstract.Tests.Impl.Device.Settings;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands; 

public class MyParamReader : StringReader<MyParam> {
    public MyParamReader(ILoggerFactory? loggerFactory = null) : base(loggerFactory) {
    }

    protected override string Compile() => $"{Cmd.PREFIX}{Cmd.QUESTION_MARK}";

    protected override MyParam Parse(string? data) {
        // assume data format = "CMD=123:456"
        var res = new MyParam();

        try {
            var split = data?.Split('=');
            var d = split?[1].Split(':');
            res.CommandPid = Convert.ToInt32(d?[0]);
            res.Value = d?[1];
        } catch(Exception e) {
            throw new CommandResultParsingException(e);
        }
        return res;
    }
}