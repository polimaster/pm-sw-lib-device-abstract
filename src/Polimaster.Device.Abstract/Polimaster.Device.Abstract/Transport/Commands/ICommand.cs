namespace Polimaster.Device.Abstract.Transport.Commands;

/// <summary>
/// Parametrized command for device with no result returned, just call & forget. 
/// </summary>
/// <typeparam name="TParam">Params value type</typeparam>
/// <typeparam name="TData">Command value type</typeparam>
public interface ICommand<TParam, out TData> {
    /// <summary>
    /// Parameters for command
    /// </summary>
    TParam Param { get; set; }

    /// <summary>
    /// Returns formatted command to be send to device
    /// </summary>
    TData Compile();
}