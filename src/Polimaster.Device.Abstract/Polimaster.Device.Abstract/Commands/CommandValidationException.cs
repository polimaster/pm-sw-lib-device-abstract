using System;

namespace Polimaster.Device.Abstract.Commands;

public class CommandValidationException : Exception {
    public CommandValidationException(Exception exception) : base("Error while validating command", exception) {
    }
}