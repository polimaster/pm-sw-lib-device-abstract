using System;

namespace Polimaster.Device.Abstract.Commands;

public class DeviceReadingException : Exception {
    public override string Message => "Device read operation failed";
}