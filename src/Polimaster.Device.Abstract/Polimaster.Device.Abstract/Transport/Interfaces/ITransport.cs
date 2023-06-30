using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Transport.Interfaces;

/// <summary>
/// Device transport layer (USB, Tcp, Bluetooth etc)
/// </summary>
/// <typeparam name="TData">Data type for device communication</typeparam>
public interface ITransport<TData> : IDisposable {
    
    string ConnectionId { get; }
    
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
    Task<Stream?> Open();

    /// <summary>
    /// Close connection
    /// </summary>
    Task Close();
}

/// <inheritdoc cref="ITransport{TData}"/>
/// <typeparam name="TConnectionParams">Parameters while connecting to device</typeparam>
/// <typeparam name="TData"><see cref="ITransport{TData}"/></typeparam>
public interface ITransport<TData, TConnectionParams> : ITransport<TData> {
    
    IClient<TConnectionParams> Client { get; }

    /// <summary>
    /// Parameters for connection
    /// </summary>
    TConnectionParams? ConnectionParams { get; }
    
}