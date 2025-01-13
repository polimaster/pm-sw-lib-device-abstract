using System;
using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Implementations.Status;

/// <summary>
/// Device online status
/// </summary>
/// <typeparam name="TStatus">Status data type</typeparam>
public interface IDeviceStatus<TStatus> {
    /// <summary>
    /// Read device status.
    /// </summary>
    /// <param name="token">The token to monitor for cancellation requests.</param>
    /// <returns></returns>
    Task<TStatus> Read(CancellationToken token);

    /// <summary>
    /// Start watch for status changes
    /// </summary>
    /// <param name="token">The token to monitor for cancellation requests.</param>
    /// <returns></returns>
    void Start(CancellationToken token);

    /// <summary>
    /// Cancels watch for status changes
    /// </summary>
    /// <returns></returns>
    void Stop();

    /// <summary>
    /// Occurs when new data got from device.
    /// Ensure <see cref="Start"/> called to get any changes.
    /// </summary>
    public event Action<TStatus> HasNext;
}