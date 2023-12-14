using System.Net;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Transport; 

public class ClientParams : IPEndPoint, IStringify {

    public ClientParams(long address, int port) : base(address, port) {
    }

    public ClientParams(IPAddress address, int port) : base(address, port) {
    }
}