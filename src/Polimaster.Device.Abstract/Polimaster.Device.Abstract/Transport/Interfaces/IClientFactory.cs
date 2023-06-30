namespace Polimaster.Device.Abstract.Transport.Interfaces;

public interface IClientFactory {
    
    /// <summary>
    /// Returns new client
    /// </summary>
    /// <typeparam name="T">Base interface for returning client</typeparam>
    /// <typeparam name="TConnectionParams"><see cref="IClient{TConnectionParams}"/></typeparam>
    /// <returns></returns>
    T CreateClient<T,TConnectionParams>() where T : IClient<TConnectionParams>;
}