using System;

namespace Polimaster.Device.Abstract.Device;

/// <summary>
/// Exception while device communication occurs
/// </summary>
public class DeviceException : Exception {
    /// <inheritdoc />
    public DeviceException(Exception exception) : base("Error while device communication", exception) {
    }

    /// <inheritdoc />
    public DeviceException(string message) : base(message) {
    }
}