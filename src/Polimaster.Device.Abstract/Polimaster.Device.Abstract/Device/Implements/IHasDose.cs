using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Implements; 

/// <summary>
/// Identifies a device with reset dose function
/// </summary>
public interface IHasDose {
    /// <summary>
    /// Resets accumulated dose on device
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task ResetDose(CancellationToken cancellationToken = new());
}