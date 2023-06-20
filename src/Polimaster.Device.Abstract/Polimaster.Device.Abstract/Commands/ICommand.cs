using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Commands;

/// <summary>
/// Parametrized command for device with no result returned, just call & forget. 
/// </summary>
/// <see cref="ITransport{TData,TConnectionParams}"/>
/// <typeparam name="TParam">Params value type</typeparam>
/// <typeparam name="TData">
/// Command value type
/// </typeparam>
public interface ICommand<TParam, out TData> {
    /// <summary>
    /// Parameters for command
    /// </summary>
    TParam? Param { get; set; }

    /// <summary>
    /// Returns formatted command to be send to device
    /// </summary>
    TData Compile();

    /// <summary>
    /// Validates command or/and its parameters before execution.
    /// Should throw CommandValidationException if failed.
    /// </summary>
    /// <exception cref="CommandValidationException"></exception>
    void Validate();
}