using System;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Transport.Stream;

namespace Polimaster.Device.Abstract.Transport;

/// <summary>
/// Client which make connection to device
/// </summary>
public interface IClient : IDisposable, IEquatable<IClient> {

    /// <summary>
    /// Connection identifier
    /// </summary>
    string ConnectionId { get; }

    /// <summary>
    /// Returns true if client connected
    /// </summary>
    bool Connected { get; }

    /// <summary>
    /// Close connection
    /// </summary>
    void Close();

    /// <summary>
    /// Get device stream for read/write operations
    /// </summary>
    /// <returns><see cref="IDeviceStream"/></returns>
    IDeviceStream GetStream();

    /// <summary>
    /// Open connection
    /// </summary>
    void Open();

    /// <summary>
    /// Open connection
    /// </summary>
    /// <param name="token">The token to monitor for cancellation requests.</param>
    /// <returns></returns>
    Task OpenAsync(CancellationToken token);

    /// <summary>
    /// Reset internal connection
    /// </summary>
    void Reset();
}