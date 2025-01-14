using System.Text;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands; 

public class DeviceInfoReader(ITransport transport, ILoggerFactory? loggerFactory)
    : ADataReader<DeviceInfo>(transport, loggerFactory) {
    protected override byte[] Compile() => Encoding.UTF8.GetBytes($"{Cmd.PREFIX}{Cmd.QUESTION_MARK}INFO");

    protected override DeviceInfo Parse(byte[] res) {
        var str = Encoding.UTF8.GetString(res);
        return new DeviceInfo { Id = str, Model = "MY_DEVICE", Modification = "TEST", Serial = str };
    }
}