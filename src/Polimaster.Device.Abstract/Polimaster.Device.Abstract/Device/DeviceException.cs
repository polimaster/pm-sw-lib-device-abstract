using System;

namespace Polimaster.Device.Abstract.Device;

/// <summary>
/// Exception while device communication occurs
/// </summary>
public class DeviceException : Exception {
    public DeviceException(Exception exception) : base("Error while device communication", exception) {
    }

    public DeviceException(string message) : base(message) {
    }
}