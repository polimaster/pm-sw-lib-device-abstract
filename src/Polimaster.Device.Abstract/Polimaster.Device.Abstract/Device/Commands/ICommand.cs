using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <summary>
/// Device command
/// </summary>
/// <typeparam name="T">Type of <see cref="IDeviceStream{T}"/></typeparam>
/// <typeparam name="TValue"></typeparam>
public interface ICommand<TValue, T> {
    /// <summary>
    /// Send command to device
    /// </summary>
    /// <param name="stream">Device stream</param>
    /// <param name="value"></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns></returns>
    Task<TValue?> Send<TStream>(TStream stream, TValue? value = default, CancellationToken cancellationToken = new()) where TStream : IDeviceStream<T>;
}