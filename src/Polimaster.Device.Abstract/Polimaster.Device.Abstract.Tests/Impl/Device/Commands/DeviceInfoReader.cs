using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device;
using Polimaster.Device.Abstract.Device.Commands.Impl;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands; 

public class DeviceInfoReader(ITransport<string> transport, ILoggerFactory? loggerFactory)
    : StringReader<DeviceInfo>(transport, loggerFactory) {
    protected override string Compile() => $"{Cmd.PREFIX}{Cmd.QUESTION_MARK}INFO";

    protected override DeviceInfo Parse(string? res) {
        return new DeviceInfo { Id = res, Model = "MY_DEVICE", Modification = "TEST", Serial = res };
    }
}