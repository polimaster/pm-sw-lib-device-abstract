using Polimaster.Device.Abstract.Device;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Device;

public class MyDevice : ADevice<string, string> {
    public MyDevice(ITransport<string, string?> transport) : base(transport) {
    }
}