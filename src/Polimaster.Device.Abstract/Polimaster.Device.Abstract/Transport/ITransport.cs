using System;
using System.Data;

namespace Polimaster.Device.Abstract.Transport;

/// <summary>
/// Device transport layer (USB, Tcp, Bluetooth etc)
/// </summary>
/// <typeparam name="TData">Data type for device communication</typeparam>
public interface ITransport<TData> : IDisposable {
    
    /// <summary>
    /// State of connection
    /// </summary>
    ConnectionState ConnectionState { get; }

    
    /// <summary>
    /// Occurs when connection state changed
    /// </summary>
    event Action<ConnectionState> ConnectionStateChanged;
    
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


    /// <summary>
    /// Connect to device
    /// </summary>
    void Open();

    /// <summary>
    /// Disconnect from device
    /// </summary>
    void Close();
}