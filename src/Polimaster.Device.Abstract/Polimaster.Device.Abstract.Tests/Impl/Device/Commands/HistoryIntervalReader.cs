using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands; 

public class HistoryIntervalReader(ITransport transport, ILoggerFactory? loggerFactory) : ADataReader<ushort?>(transport, loggerFactory) {
    protected override byte[] Compile() => Encoding.UTF8.GetBytes($"{Cmd.PREFIX}{Cmd.QUESTION_MARK}INTERVAL");

    protected override ushort? Parse(byte[] res) => BitConverter.ToUInt16(res);
}