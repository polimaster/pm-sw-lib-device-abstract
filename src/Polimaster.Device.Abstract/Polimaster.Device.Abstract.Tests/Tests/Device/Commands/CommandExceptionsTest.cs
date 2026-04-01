using System;
using Polimaster.Device.Abstract.Device.Commands.Exceptions;

namespace Polimaster.Device.Abstract.Tests.Tests.Device.Commands;

public class CommandExceptionsTest {
    [Fact]
    public void CommandCompilationException_WithMessage() {
        var msg = "test message";
        var ex = new CommandCompilationException(msg);
        Assert.Equal(msg, ex.Message);
    }

    [Fact]
    public void CommandCompilationException_WithException() {
        var inner = new Exception("inner");
        var ex = new CommandCompilationException(inner);
        Assert.Contains("Error while compiling command", ex.Message);
        Assert.Equal(inner, ex.InnerException);
    }

    [Fact]
    public void CommandResultParsingException_WithMessage() {
        var msg = "test message";
        var ex = new CommandResultParsingException(msg);
        Assert.Equal(msg, ex.Message);
    }

    [Fact]
    public void CommandResultParsingException_WithException() {
        var inner = new Exception("inner");
        var ex = new CommandResultParsingException(inner);
        Assert.Contains("Error while parsing result of command", ex.Message);
        Assert.Equal(inner, ex.InnerException);
    }
}
