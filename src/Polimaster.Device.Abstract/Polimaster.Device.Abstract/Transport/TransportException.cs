using System;

namespace Polimaster.Device.Abstract.Transport;

public class TransportException : Exception {
    public override string Message => "Transport connection closed or broken";
}