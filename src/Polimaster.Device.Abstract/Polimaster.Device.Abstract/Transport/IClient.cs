using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Transport;

/// <summary>
/// Client which make connection to device
/// </summary>
/// <typeparam name="TConnectionParams">Type of device connection parameters</typeparam>
public interface IClient<in TConnectionParams> : IDisposable {

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
    Task<IDeviceStream> GetStream();

    /// <summary>
    /// Open connection
    /// </summary>
    /// <param name="connectionParams"></param>
    void Open(TConnectionParams connectionParams);

    /// <summary>
    /// Open connection
    /// </summary>
    /// <param name="connectionParams"></param>
    /// <returns></returns>
    Task OpenAsync(TConnectionParams connectionParams);
}