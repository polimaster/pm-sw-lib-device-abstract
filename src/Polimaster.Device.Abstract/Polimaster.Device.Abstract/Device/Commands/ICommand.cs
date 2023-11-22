using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <summary>
/// Device command
/// </summary>
public interface ICommand {
    /// <summary>
    /// Sends command to device stream
    /// </summary>
    /// <param name="stream">Transport <see cref="IDeviceStream{T}"/></param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <typeparam name="TStream">Stream type</typeparam>
    /// <returns></returns>
    Task Exec<TStream>(TStream stream, CancellationToken cancellationToken);
}