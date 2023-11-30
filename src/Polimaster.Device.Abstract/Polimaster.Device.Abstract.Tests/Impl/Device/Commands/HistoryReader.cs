using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Impl;
using Polimaster.Device.Abstract.Tests.Impl.Device.History;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands; 


public struct HistoryReaderChunk {
    public IEnumerable<HistoryRecord>? Records;
    public bool HasReachedTheEnd;

    public HistoryReaderChunk() {
        Records = null;
        HasReachedTheEnd = false;
    }
}

public class HistoryReader : StringReader<HistoryReaderChunk> {
    public HistoryReader(ILoggerFactory? loggerFactory) : base(loggerFactory) {
    }

    protected override string Compile() => $"{Cmd.PREFIX}{Cmd.QUESTION_MARK}HISTORY";

    protected override HistoryReaderChunk Parse(string res) {
        return new HistoryReaderChunk {
            HasReachedTheEnd = true,
            Records = new List<HistoryRecord>()
        };
    }
}