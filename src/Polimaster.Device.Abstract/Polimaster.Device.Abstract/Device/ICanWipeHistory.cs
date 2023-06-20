using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device;

/// <summary>
/// Identifies a device with a history that can be wiped from device
/// </summary>
public interface ICanWipeHistory {
    Task WipeHistory<TParams>(TParams parameters);
    Task WipeHistory();
}