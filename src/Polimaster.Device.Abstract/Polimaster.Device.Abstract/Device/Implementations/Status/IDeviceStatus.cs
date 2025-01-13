using System;
using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Implementations.Status;

/// <summary>
/// Device online status
/// </summary>
/// <typeparam name="T">Status data type</typeparam>
public interface IDeviceStatus<T> {
    /// <summary>
    /// Read device status.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<T> Read(CancellationToken token = new());

    /// <summary>
    /// Start watch for status changes
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    void Start(CancellationToken token = new());

    /// <summary>
    /// Cancels watch for status changes
    /// </summary>
    /// <returns></returns>
    void Stop();

    /// <summary>
    /// Occurs when new data got from device.
    /// Ensure <see cref="Start"/> called to get any changes.
    /// </summary>
    public event Action<T> HasNext;
}