using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device;
using Polimaster.Device.Abstract.Device.Commands.Impl;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands; 

public class DeviceInfoReader : StringReader<DeviceInfo> {
    public DeviceInfoReader(ILoggerFactory? loggerFactory) : base(loggerFactory) {
    }

    protected override string Compile() => $"{Cmd.PREFIX}{Cmd.QUESTION_MARK}INFO";

    protected override DeviceInfo Parse(string res) {
        return new DeviceInfo { Id = res, Model = "MY_DEVICE", Modification = "TEST", Serial = res };
    }
}