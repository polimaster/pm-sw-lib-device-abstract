using System;

namespace Polimaster.Device.Abstract.Transport;

public class DeviceClientException : Exception {
    /// <inheritdoc />
    public DeviceClientException(string message) : base(message) {
    }
}