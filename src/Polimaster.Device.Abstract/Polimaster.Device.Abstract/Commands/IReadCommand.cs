using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Commands; 

/// <summary>
/// Parametrized command for device with command result returned.
/// </summary>
/// <typeparam name="TResult">Type of command result</typeparam>
/// <typeparam name="TParam">
/// <see cref="ICommand{TParam,TData}"/>
/// </typeparam>
/// <typeparam name="TData">
/// <see cref="ICommand{TParam,TData}"/>
/// </typeparam>
public interface IReadCommand<out TResult, TParam, TData> : ICommand<TParam, TData> {
    
    /// <summary>
    /// This method should parse result of command
    /// </summary>
    /// <param name="result">
    /// <see cref="ITransport{TData,TConnectionParams}.Read"/>
    /// Result of executed command
    /// </param>
    TResult? Parse(TData result);
}