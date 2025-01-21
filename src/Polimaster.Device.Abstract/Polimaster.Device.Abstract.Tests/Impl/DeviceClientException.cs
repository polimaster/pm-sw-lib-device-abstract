using System;

namespace Polimaster.Device.Abstract.Tests.Impl;

/// <inheritdoc />
public class DeviceClientException : Exception {
    /// <inheritdoc />
    public DeviceClientException(string message) : base(message) {
    }
}