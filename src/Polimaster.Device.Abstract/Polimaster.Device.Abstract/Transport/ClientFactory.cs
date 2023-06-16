namespace Polimaster.Device.Abstract.Transport;

public class ClientFactory : IClientFactory {
    public T CreateClient<T, TConnectionParams>() where T : IClient<TConnectionParams>, new() {
        return new T();
    }
}