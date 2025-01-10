using System;
using System.Net;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Transport; 

public class ClientParams : IPEndPoint, IFormattable {

    public ClientParams(long address, int port) : base(address, port) {
    }

    public ClientParams(IPAddress address, int port) : base(address, port) {
    }

    public string ToString(string? format, IFormatProvider? formatProvider) {
        return base.ToString();
    }
}