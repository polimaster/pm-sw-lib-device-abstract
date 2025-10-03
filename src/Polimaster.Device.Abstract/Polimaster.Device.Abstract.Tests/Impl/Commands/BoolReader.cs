using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Tests.Impl.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Commands;

public class BoolReader(IMyTransport transport, ILoggerFactory? loggerFactory) : MyDeviceStreamReader<bool>(transport, loggerFactory) {
    protected override byte[] Compile() => Encoding.UTF8.GetBytes($"{Cmd.PREFIX}{Cmd.QUESTION_MARK}BOOL");

    protected override bool Parse(byte[]? res) {
        var v = BitConverter.ToUInt16(res ?? throw new ArgumentNullException(nameof(res)), 0);
        return v == 1;
    }
}