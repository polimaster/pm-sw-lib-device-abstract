using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Implementations.History;
using Polimaster.Device.Abstract.Tests.Impl.Device.Commands;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.History; 

public class HistoryManager(ITransport<string> transport, ILoggerFactory? loggerFactory)
    : AHistoryManager<string, HistoryRecord>(transport, loggerFactory) {
    private readonly HistoryReader _historyReader = new(transport, loggerFactory);
    private readonly HistoryWiper _historyWiper = new(transport, loggerFactory);
    private CancellationTokenSource? _readCancellationToken;

    public override event Action<HistoryChunk<HistoryRecord>>? HasNext;

    public override async Task Read(CancellationToken token = new()) {
        if (_readCancellationToken != null) {
            Logger?.LogDebug("Reading history is already in progress");
            return;
        }
        _readCancellationToken = CancellationTokenSource.CreateLinkedTokenSource(token);
        
        var rows = new List<HistoryRecord>();
        var hasReachedTheEndOfData = false;
        while (!_readCancellationToken.IsCancellationRequested) {
            var res = await _historyReader.Read(_readCancellationToken.Token);
            if (res.Records != null) rows.AddRange(res.Records);
            if (!res.HasReachedTheEnd) continue;
            
            hasReachedTheEndOfData = true;
            break;
        }
        
        // device didn't return all data, so history can't be parsed
        if (_readCancellationToken.IsCancellationRequested && !hasReachedTheEndOfData) {
            HasNext?.Invoke(new HistoryChunk<HistoryRecord> { Completed = true });
            _readCancellationToken = null;
            return;
        }

        HasNext?.Invoke(new HistoryChunk<HistoryRecord> { Completed = true, Records = rows });
        _readCancellationToken = null;
    }

    public override void Stop() => _readCancellationToken?.Cancel();

    public override async Task Wipe(CancellationToken token = new()) =>
        await _historyWiper.Exec(token);
}