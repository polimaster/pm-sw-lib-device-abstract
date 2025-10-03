using System.Text;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Tests.Impl.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Commands;

public class OnOffWriter(IMyTransport transport, ILoggerFactory? loggerFactory) : MyDeviceStreamWriter<OnOff>(transport, loggerFactory) {
    protected override byte[] Compile(OnOff data) {
        var v = data == OnOff.ON ? 1 : 0;
        return Encoding.UTF8.GetBytes($"{Cmd.PREFIX}ONOFF:{v}");
    }
}