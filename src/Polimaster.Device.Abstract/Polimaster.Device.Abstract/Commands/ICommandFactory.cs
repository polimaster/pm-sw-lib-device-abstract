using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Commands;

public interface ICommandFactory<TData> {
    T Create<T>() where T : ITransportCommand<TData>, new();
}