using System;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Exceptions;
using Polimaster.Device.Abstract.Device.Commands.Impl;
using Polimaster.Device.Abstract.Tests.Impl.Device.Settings;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands;

public class MyParamWriter(ITransport<string> transport, ILoggerFactory? loggerFactory = null) : StringWriter<MyParam?>(transport, loggerFactory) {
    protected override string Compile(MyParam? data) {
        if (data == null) {
            throw new CommandCompilationException("Data is null", new NullReferenceException());
        }
        return $"{Cmd.PREFIX}{data.CommandPid}:{data.Value}";
    }
}