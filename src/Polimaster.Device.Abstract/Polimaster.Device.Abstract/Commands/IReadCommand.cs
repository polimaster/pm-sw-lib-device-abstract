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
    /// Result of executing command
    /// </summary>
    TResult? Result { get; }

    /// <summary>
    /// This method should parse result of command
    /// </summary>
    /// <param name="result">
    /// <see cref="Transport.ITransport{TData}"/>
    /// Result returned from transport
    /// </param>
    void Parse(TData result);
}