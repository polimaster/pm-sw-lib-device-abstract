using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <summary>
/// Device command
/// </summary>
public interface ICommand {
    /// <summary>
    /// Sends command to device stream
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns></returns>
    Task Exec(CancellationToken cancellationToken);
}