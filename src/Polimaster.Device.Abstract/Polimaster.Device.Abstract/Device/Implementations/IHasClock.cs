using System;
using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Implementations; 

/// <summary>
/// Identifies a device with internal clock
/// </summary>
public interface IHasClock {
    /// <summary>
    /// Sets date and time on device
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <param name="dateTime">Time</param>
    /// <returns></returns>
    Task SetTime(CancellationToken cancellationToken, DateTime? dateTime = null);

    /// <summary>
    /// Reads current date and time from device
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>Current date and time on device</returns>
    Task<DateTime?> GetTime(CancellationToken cancellationToken);
}