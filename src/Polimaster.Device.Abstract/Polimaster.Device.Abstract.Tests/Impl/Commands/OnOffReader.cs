using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Tests.Impl.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Commands;

public class OnOffReader(IMyTransport transport, ILoggerFactory? loggerFactory) : MyDeviceStreamReader<OnOff>(transport, loggerFactory) {
    protected override byte[] Compile() => Encoding.UTF8.GetBytes($"{Cmd.PREFIX}{Cmd.QUESTION_MARK}ONOFF");

    protected override OnOff Parse(byte[]? res) {
        var v = BitConverter.ToUInt16(res ?? throw new ArgumentNullException(nameof(res)), 0);
        return v == 1 ? OnOff.ON : OnOff.OFF;
    }
}