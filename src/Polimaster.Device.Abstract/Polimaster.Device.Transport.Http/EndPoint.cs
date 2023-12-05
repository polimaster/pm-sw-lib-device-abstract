using System.Net;
using Polimaster.Device.Abstract;

namespace Polimaster.Device.Transport.Http;

/// <inheritdoc cref="System.Net.IPEndPoint" />
public class EndPoint : IPEndPoint, IStringify {
    /// <inheritdoc />
    public EndPoint(long address, int port) : base(address, port) {
    }

    /// <inheritdoc />
    public EndPoint(IPAddress address, int port) : base(address, port) {
    }
}