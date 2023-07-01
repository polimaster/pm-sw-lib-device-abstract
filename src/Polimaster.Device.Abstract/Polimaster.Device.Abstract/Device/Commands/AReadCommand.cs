using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device.Commands.Interfaces;
using Polimaster.Device.Abstract.Transport.Interfaces;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <summary>
/// Command updates <see cref="ICommand{T,TTranspor}.Value"/> while <see cref="ITransport{TData}.Read"/>
/// </summary>
/// <inheritdoc cref="AWriteCommand{T,TTransport}"/>
public abstract class AReadCommand<T, TTransport> : AWriteCommand<T, TTransport> {
    protected abstract T Parse(TTransport data);
    public override async Task Send(CancellationToken cancellationToken = new()) {
        var stream = await Prepare();
        if (cancellationToken.IsCancellationRequested) return;
        var res = await Transport!.Read(stream, Compile(), cancellationToken);
        Value = Parse(res);
        ValueChanged?.Invoke(Value);
    }
}