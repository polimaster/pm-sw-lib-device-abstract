using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Tests.Impl.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Commands; 

public class HistoryIntervalReader(IMyTransport transport, ILoggerFactory? loggerFactory) : MyDeviceStreamReader<ushort>(transport, loggerFactory) {
    protected override byte[] Compile() => Encoding.UTF8.GetBytes($"{Cmd.PREFIX}{Cmd.QUESTION_MARK}INTERVAL");

    protected override ushort Parse(byte[]? res) {
        return BitConverter.ToUInt16(res ?? throw new ArgumentNullException(nameof(res)), 0);
    }
}