using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Exceptions;
using Polimaster.Device.Abstract.Tests.Impl.Settings;
using Polimaster.Device.Abstract.Tests.Impl.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Commands;

public class MyParamWriter(IMyTransport transport, ILoggerFactory? loggerFactory = null) : MyDeviceStreamWriter<MyParam>(transport, loggerFactory) {
    protected override byte[] Compile(MyParam data) {
        if (data == null) {
            throw new CommandCompilationException(new NullReferenceException("Data cannot be null."));
        }
        return Encoding.UTF8.GetBytes($"{Cmd.PREFIX}{data.CommandPid}:{data.Value}");
    }
}