using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Tests.Impl.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Commands; 

public class TimeReader(IMyTransport transport, ILoggerFactory? loggerFactory)
    : MyDeviceStreamReader<DateTime>(transport, loggerFactory) {
    protected override byte[] Compile() => Encoding.UTF8.GetBytes($"{Cmd.PREFIX}{Cmd.QUESTION_MARK}TIME");

    protected override DateTime Parse(byte[]? res) {
        return DateTime.Now;
    }
}