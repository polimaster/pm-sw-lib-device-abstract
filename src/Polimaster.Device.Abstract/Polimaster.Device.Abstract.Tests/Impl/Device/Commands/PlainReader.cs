using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Exceptions;
using Polimaster.Device.Abstract.Device.Commands.Impl;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands;

public class PlainReader(ITransport transport, ILoggerFactory? loggerFactory) : StringReader(transport, loggerFactory) {
    protected override byte[] Compile() => Encoding.UTF8.GetBytes($"{Cmd.PREFIX}{Cmd.QUESTION_MARK}");

    protected override string Parse(byte[] res) {
        if (res == null) {
            throw new CommandResultParsingException(new NullReferenceException());
        }
        return Encoding.UTF8.GetString(res);
    }
}