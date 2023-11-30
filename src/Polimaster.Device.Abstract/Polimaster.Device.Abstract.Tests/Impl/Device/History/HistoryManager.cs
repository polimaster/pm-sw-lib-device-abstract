using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Implementations.History;
using Polimaster.Device.Abstract.Tests.Impl.Device.Commands;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.History; 

public class HistoryManager : AHistoryManager<HistoryRecord> {
    private readonly HistoryReader _historyReader;
    private readonly HistoryWiper _historyWiper;
    private CancellationTokenSource? _readCancellationToken;

    public HistoryManager(ILoggerFactory? loggerFactory) : base(loggerFactory) {
        _historyReader = new HistoryReader(loggerFactory);
        _historyWiper = new HistoryWiper(loggerFactory);
    }
    
    public override async Task Read(ITransport transport, CancellationToken token = new()) {
        if (_readCancellationToken != null) {
            Logger?.LogDebug("Reading history is already in progress");
            return;
        }
        _readCancellationToken = CancellationTokenSource.CreateLinkedTokenSource(token);
        
        var rows = new List<HistoryRecord>();
        var hasReachedTheEndOfData = false;
        while (!_readCancellationToken.IsCancellationRequested) {
            var res = await transport.Read(_historyReader, _readCancellationToken.Token);
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

    public override void Stop() {
        _readCancellationToken?.Cancel();
        _readCancellationToken = null;
    }

    public override async Task Wipe(ITransport transport, CancellationToken token = new()) => 
        await transport.Exec(_historyWiper, token);
}