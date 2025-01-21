using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Tests.Impl.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Commands; 

public class TimeWriter(IMyTransport transport, ILoggerFactory? loggerFactory)
    : MyDeviceStreamWriter<DateTime>(transport, loggerFactory) {
    protected override byte[] Compile(DateTime data) => Encoding.UTF8.GetBytes($"{Cmd.PREFIX}TIME:{data}");
}