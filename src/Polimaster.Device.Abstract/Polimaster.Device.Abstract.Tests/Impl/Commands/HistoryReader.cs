using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Tests.Impl.History;
using Polimaster.Device.Abstract.Tests.Impl.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Commands; 


public struct HistoryReaderChunk {
    public IEnumerable<HistoryRecord>? Records;
    public bool HasReachedTheEnd;

    public HistoryReaderChunk() {
        Records = null;
        HasReachedTheEnd = false;
    }
}

public class HistoryReader(IMyTransport transport, ILoggerFactory? loggerFactory)
    : MyDeviceStreamReader<HistoryReaderChunk>(transport, loggerFactory) {
    protected override byte[] Compile() => Encoding.UTF8.GetBytes($"{Cmd.PREFIX}{Cmd.QUESTION_MARK}HISTORY");

    protected override HistoryReaderChunk Parse(byte[]? res) {
        return new HistoryReaderChunk {
            HasReachedTheEnd = true,
            Records = new List<HistoryRecord>()
        };
    }
}