using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device.Commands.Interfaces;
using Polimaster.Device.Abstract.Transport.Interfaces;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <summary>
/// Command updates <see cref="ICommand{TValue,TTransportData}.Value"/> while <see cref="ITransport{TData}.Read"/>
/// </summary>
/// <typeparam name="TValue"><inheritdoc cref="ICommand{TValue,TTransportData}"/></typeparam>
/// <typeparam name="TTransportData"><inheritdoc cref="ICommand{TValue,TTransportData}"/></typeparam>
public abstract class AReadCommand<TValue, TTransportData> : AWriteCommand<TValue, TTransportData> {
    protected abstract TValue Parse(TTransportData data);
    public override async Task Send(CancellationToken cancellationToken = new()) {
        var stream = await Prepare();
        if (cancellationToken.IsCancellationRequested) return;
        var res = await Transport!.Read(stream, Compile(), cancellationToken);
        Value = Parse(res);
        ValueChanged?.Invoke(Value);
    }
}