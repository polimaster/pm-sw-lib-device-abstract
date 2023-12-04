using System;
using System.Net;

namespace Polimaster.Device.Transport.Http;

/// <inheritdoc cref="System.Net.IPEndPoint" />
public class EndPoint : IPEndPoint, IFormattable {
    /// <inheritdoc />
    public EndPoint(long address, int port) : base(address, port) {
    }

    /// <inheritdoc />
    public EndPoint(IPAddress address, int port) : base(address, port) {
    }
    

    /// <inheritdoc />
    public override string ToString() => ToString(null, System.Globalization.CultureInfo.CurrentCulture);

    /// <inheritdoc />
    public string ToString(string? format, IFormatProvider formatProvider) => base.ToString();
}