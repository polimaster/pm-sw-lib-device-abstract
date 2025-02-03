using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device;
using Polimaster.Device.Abstract.Tests.Impl.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Commands;

public class DeviceInfoReader(IMyTransport transport, ILoggerFactory? loggerFactory)
    : MyDeviceStreamReader<DeviceInfo>(transport, loggerFactory) {
    protected override byte[] Compile() => Encoding.UTF8.GetBytes($"{Cmd.PREFIX}{Cmd.QUESTION_MARK}INFO");

    protected override DeviceInfo Parse(byte[]? res) {
        ArgumentNullException.ThrowIfNull(res);
        var str = Encoding.UTF8.GetString(res);
        return new DeviceInfo { Id = str, Model = "MY_DEVICE", Modification = "TEST", Serial = str };
    }
}