using System;

namespace Polimaster.Device.Abstract.Transport;

/// <summary>
/// Transport exception
/// </summary>
public class TransportException : Exception {
    /// <inheritdoc />
    public TransportException(string? message) {
        if (message != null) Message = message;
    }

    /// <inheritdoc />
    public override string Message { get; } = "Transport connection closed or broken";
}