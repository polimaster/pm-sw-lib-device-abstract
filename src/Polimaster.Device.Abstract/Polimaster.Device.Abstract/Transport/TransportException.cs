using System;

namespace Polimaster.Device.Abstract.Transport;

public class TransportException : Exception {
    public TransportException(string? message) {
        if (message != null) Message = message;
    }

    public override string Message { get; } = "Transport connection closed or broken";
}