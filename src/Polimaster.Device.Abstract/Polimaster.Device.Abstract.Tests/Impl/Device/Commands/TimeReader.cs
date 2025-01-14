using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands; 

public class TimeReader(ITransport transport, ILoggerFactory? loggerFactory)
    : ADataReader<DateTime>(transport, loggerFactory) {
    protected override byte[] Compile() => Encoding.UTF8.GetBytes($"{Cmd.PREFIX}{Cmd.QUESTION_MARK}TIME");

    protected override DateTime Parse(byte[] res) {
        return DateTime.Now;
    }
}