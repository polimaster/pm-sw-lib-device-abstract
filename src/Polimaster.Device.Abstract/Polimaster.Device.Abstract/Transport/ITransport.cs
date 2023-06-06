using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Transport;

/// <summary>
/// Device transport layer (USB, Tcp, Bluetooth etc)
/// </summary>
/// <typeparam name="TData">Data type for device communication</typeparam>
public interface ITransport<TData, TConnectionParams> : IDisposable {
    
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
    /// <param name="stream"></param>
    /// <param name="command">Command</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    Task Write(Stream stream, TData command, CancellationToken cancellationToken);

    /// <summary>
    /// Read well-formatted command to device
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="command">Command</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns>Result of command</returns>
    Task<TData> Read(Stream stream, TData command, CancellationToken cancellationToken);


    /// <summary>
    /// Open connection
    /// </summary>
    Task<Stream> Open(TConnectionParams connectionParams);

    /// <summary>
    /// Close connection
    /// </summary>
    Task Close();
}