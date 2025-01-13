using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Implementations; 

/// <summary>
/// Identifies a device with reset dose function
/// </summary>
public interface IHasDose {
    /// <summary>
    /// Resets accumulated dose on device
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns></returns>
    Task ResetDose(CancellationToken cancellationToken);
}