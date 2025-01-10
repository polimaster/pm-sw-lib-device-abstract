using System;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device.Commands;

namespace Polimaster.Device.Abstract.Transport;

/// <summary>
/// Device transport layer (USB, Tcp, Bluetooth etc.)
/// </summary>
public interface ITransport<T> : IDisposable {
    /// <summary>
    /// Connection identifier
    /// </summary>
    // string ConnectionId { get; }

    IClient<T> Client { get; }

    /// <summary>
    /// Indicates connection will be closed
    /// </summary>
    public event Action? Closing;

    /// <summary>
    /// Open device connection
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task OpenAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Open device connection
    /// </summary>
    void Open();

    /// <summary>
    /// Close connection
    /// </summary>
    void Close();

    /// <summary>
    /// Write data with <see cref="IDataWriter{T}"/>
    /// </summary>
    /// <param name="data">Data to write</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    Task WriteAsync(T data, CancellationToken cancellationToken);
    
    /// <summary>
    /// Read data with <see cref="IDataReader{T}"/>
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    Task<T> ReadAsync(CancellationToken cancellationToken);
}