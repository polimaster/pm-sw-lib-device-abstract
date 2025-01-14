using System;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Transport.Stream;

namespace Polimaster.Device.Abstract.Transport;

/// <summary>
/// Device transport layer (USB, Tcp, Bluetooth etc.)
/// </summary>
public interface ITransport : IDisposable {
    /// <summary>
    /// Connection identifier
    /// </summary>
    // string ConnectionId { get; }

    IClient Client { get; }

    /// <summary>
    /// Indicates connection will be closed
    /// </summary>
    public event Action? Closing;

    /// <summary>
    /// Open device connection
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
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
    /// Write data
    /// </summary>
    /// <param name="data">Data to write</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <param name="channel">See <see cref="IDeviceStream.WriteAsync{T}"/></param>
    /// <returns></returns>
    Task WriteAsync<T>(byte[] data, CancellationToken cancellationToken, T? channel = default);

    /// <summary>
    /// Write data to default <see cref="IDeviceStream"/> channel
    /// </summary>
    /// <param name="data">Data to write</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns></returns>
    Task WriteAsync(byte[] data, CancellationToken cancellationToken);

    /// <summary>
    /// Read data
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <param name="channel">See <see cref="IDeviceStream.ReadAsync{T}"/></param>
    /// <returns></returns>
    Task<byte[]> ReadAsync<T>(CancellationToken cancellationToken, T? channel = default);

    /// <summary>
    /// Read data from default <see cref="IDeviceStream"/> channel
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns></returns>
    Task<byte[]> ReadAsync(CancellationToken cancellationToken);
}