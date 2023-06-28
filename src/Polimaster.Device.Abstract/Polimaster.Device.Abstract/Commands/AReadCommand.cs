using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Commands;

public abstract class AReadCommand<TValue, TTransportData> : AWriteCommand<TValue, TTransportData> {
    protected abstract TValue Parse(TTransportData data);
    public override async Task Send(CancellationToken cancellationToken = new()) {
        var stream = await Prepare();
        if (cancellationToken.IsCancellationRequested) return;
        var res = await Transport!.Read(stream, Compile(), cancellationToken);
        Value = Parse(res);
    }
}