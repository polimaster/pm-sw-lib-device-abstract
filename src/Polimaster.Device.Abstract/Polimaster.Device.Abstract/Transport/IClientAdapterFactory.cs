namespace Polimaster.Device.Abstract.Transport;

public interface IClientAdapterFactory {
    T CreateClient<T,TConnectionParams>() where T : IClient<TConnectionParams>, new();
}