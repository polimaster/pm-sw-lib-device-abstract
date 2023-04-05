namespace Polimaster.Device.Abstract.Transport;

/// <summary>
/// Transport layer
/// </summary>
/// <typeparam name="TData">Data type for device communication</typeparam>
public interface ITransport<TData> {
    
    /// <summary>
    /// Write well-formatted command to device
    /// </summary>
    /// <param name="command">Command</param>
    void Write(TData command);
    
    /// <summary>
    /// Read well-formatted command to device
    /// </summary>
    /// <param name="command">Command</param>
    /// <returns>Result of command</returns>
    TData Read(TData command);
}