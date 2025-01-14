using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Tests.Impl.Device.History;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands; 


public struct HistoryReaderChunk {
    public IEnumerable<HistoryRecord>? Records;
    public bool HasReachedTheEnd;

    public HistoryReaderChunk() {
        Records = null;
        HasReachedTheEnd = false;
    }
}

public class HistoryReader(ITransport transport, ILoggerFactory? loggerFactory)
    : ADataReader<HistoryReaderChunk>(transport, loggerFactory) {
    protected override byte[] Compile() => Encoding.UTF8.GetBytes($"{Cmd.PREFIX}{Cmd.QUESTION_MARK}HISTORY");

    protected override HistoryReaderChunk Parse(byte[] res) {
        return new HistoryReaderChunk {
            HasReachedTheEnd = true,
            Records = new List<HistoryRecord>()
        };
    }
}