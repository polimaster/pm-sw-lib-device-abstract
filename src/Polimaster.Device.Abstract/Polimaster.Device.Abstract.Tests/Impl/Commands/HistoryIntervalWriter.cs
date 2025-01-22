using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Tests.Impl.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Commands; 

public class HistoryIntervalWriter(IMyTransport transport, ILoggerFactory? loggerFactory) : MyDeviceStreamWriter<TimeSpan>(transport, loggerFactory) {
    protected override byte[] Compile(TimeSpan data) {
        return Encoding.UTF8.GetBytes($"{Cmd.PREFIX}INTERVAL:{data.TotalSeconds}");
    }
}