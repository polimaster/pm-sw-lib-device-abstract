using Polimaster.Device.Abstract.Transport.Interfaces;

namespace Polimaster.Device.Abstract.Device.Interfaces; 


/// <summary>
/// Device builder. Make sure you are creating singleton for each
/// <see cref="IDeviceBuilder{TTransport}"/> implementation in order to cache its data.
/// </summary>
/// <typeparam name="TTransport">Type of <see cref="ITransport{T}"/></typeparam>
public interface IDeviceBuilder<TTransport> {

    /// <summary>
    /// Add transport to device
    /// </summary>
    /// <param name="transport"><see cref="IDevice{T}.Transport"/></param>
    /// <returns><see cref="IDeviceBuilder{TTransport}"/></returns>
    IDeviceBuilder<TTransport> With(ITransport<TTransport> transport);

    /// <summary>
    /// Build device
    /// </summary>
    /// <typeparam name="T">Device implementation</typeparam>
    /// <returns><see cref="IDevice{T}"/></returns>
    T Build<T>() where T : class, IDevice<TTransport>, new();
}