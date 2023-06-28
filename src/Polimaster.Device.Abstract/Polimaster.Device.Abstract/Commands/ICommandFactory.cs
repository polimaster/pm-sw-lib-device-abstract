using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Commands;

/// <summary>
/// Factory for creating device commands
/// </summary>
/// <typeparam name="TTransportData"><see cref="ITransport{TData}"/></typeparam>
public interface ICommandFactory<TTransportData> {
    /// <summary>
    /// Create <see cref="ICommand{TValue,TTransportData}"/>
    /// </summary>
    /// <typeparam name="T">Command type</typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <returns><see cref="ICommand{TValue,TTransportData}"/></returns>
    T Create<T, TValue>() where T : ICommand<TValue, TTransportData>, new();
}