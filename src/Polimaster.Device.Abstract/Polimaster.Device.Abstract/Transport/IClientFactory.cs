namespace Polimaster.Device.Abstract.Transport;

public interface IClientFactory {
    T CreateClient<T,TConnectionParams>() where T : IClient<TConnectionParams>, new();
}