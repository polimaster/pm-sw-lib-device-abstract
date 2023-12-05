using System;

namespace Polimaster.Device.Abstract.Transport;

public class DeviceClientException : Exception {
    public DeviceClientException(string message) : base(message) {
    }
}