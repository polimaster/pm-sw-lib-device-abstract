using System;
using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Transport;


/// <summary>
/// Client that makes connection to the device
/// </summary>
/// <typeparam name="TStream">Stream type</typeparam>
public interface IClient<TStream> : IDisposable, IEquatable<IClient<TStream>> {

    /// <summary>
    /// Get device stream for read/write operations
    /// </summary>
    TStream GetStream();

    /// <summary>
    /// Connection identifier
    /// </summary>
    string ConnectionId { get; }

    /// <summary>
    /// Returns true if the client connected
    /// </summary>
    bool Connected { get; }

    /// <summary>
    /// Close connection
    /// </summary>
    void Close();

    /// <summary>
    /// Open connection
    /// </summary>
    /// <param name="token">The token to monitor for cancellation requests.</param>
    /// <returns></returns>
    Task Open(CancellationToken token);

    /// <summary>
    /// Reset internal connection
    /// </summary>
    void Reset();
}
