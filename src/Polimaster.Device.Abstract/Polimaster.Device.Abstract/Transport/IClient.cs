using System;
using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Transport;

/// <summary>
/// Client which make connection to device
/// </summary>
/// <typeparam name="T">Type for <see cref="IDeviceStream{T}"/></typeparam>
public interface IClient<T> : IDisposable, IFormattable {

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
    /// <returns><see cref="IDeviceStream{T}"/></returns>
    IDeviceStream<T> GetStream();

    /// <summary>
    /// Open connection
    /// </summary>
    void Open();

    /// <summary>
    /// Open connection
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Task OpenAsync(CancellationToken token);
}