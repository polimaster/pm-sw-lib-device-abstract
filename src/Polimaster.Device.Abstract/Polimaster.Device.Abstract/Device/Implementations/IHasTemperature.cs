using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Implementations;

/// <summary>
/// Identifies a device with temperature sensor
/// </summary>
public interface IHasTemperatureSensor {
    /// <summary>
    /// Read device temperature
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns></returns>
    Task<double?> ReadTemperature(CancellationToken cancellationToken);
}