namespace Polimaster.Device.Abstract.Transport;

public class ClientAdapterFactory : IClientAdapterFactory {
    public T CreateClient<T, TConnectionParams>() where T : IClient<TConnectionParams>, new() {
        return new T();
    }
}