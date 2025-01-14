using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Device.Commands.Exceptions;
using Polimaster.Device.Abstract.Tests.Impl.Device.Settings;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands;

public class MyParamWriter(ITransport transport, ILoggerFactory? loggerFactory = null) : ADataWriter<MyParam?>(transport, loggerFactory) {
    protected override byte[] Compile(MyParam? data) {
        if (data == null) {
            throw new CommandCompilationException("Data is null", new NullReferenceException());
        }
        return Encoding.UTF8.GetBytes($"{Cmd.PREFIX}{data.CommandPid}:{data.Value}");
    }
}