using System;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Exceptions;
using Polimaster.Device.Abstract.Device.Commands.Impl;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands;

public class PlainReader(ITransport<string> transport, ILoggerFactory? loggerFactory) : StringReader<string?>(transport, loggerFactory) {
    protected override string Compile() => $"{Cmd.PREFIX}{Cmd.QUESTION_MARK}";

    protected override string Parse(string? res) {
        if (res == null) {
            throw new CommandResultParsingException(new NullReferenceException());
        }
        return res;
    }
}