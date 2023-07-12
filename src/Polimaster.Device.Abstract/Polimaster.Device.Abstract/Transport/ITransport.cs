using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Transport;

/// <summary>
/// Device transport layer (USB, Tcp, Bluetooth etc)
/// </summary>
/// <typeparam name="T">Data type for device communication</typeparam>
public interface ITransport<T> : IDisposable {
    
    string ConnectionId { get; }
    
    /// <summary>
    /// Write well-formatted command to device
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="command">Command</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    Task Write(Stream stream, T command, CancellationToken cancellationToken);

    /// <summary>
    /// Read well-formatted command to device
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="command">Command</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns>Result of command</returns>
    Task<T> Read(Stream stream, T command, CancellationToken cancellationToken);
    
    /// <summary>
    /// Open connection
    /// </summary>
    Task<Stream?> Open();

    /// <summary>
    /// Close connection
    /// </summary>
    Task Close();
}

/// <inheritdoc cref="ITransport{TData}"/>
/// <typeparam name="TConnectionParams">Parameters while connecting to device</typeparam>
/// <typeparam name="T"><inheritdoc cref="ITransport{T}"/></typeparam>
public interface ITransport<T, TConnectionParams> : ITransport<T> {
    
    IClient<TConnectionParams> Client { get; }

    /// <summary>
    /// Parameters for connection
    /// </summary>
    TConnectionParams? ConnectionParams { get; }
    
}