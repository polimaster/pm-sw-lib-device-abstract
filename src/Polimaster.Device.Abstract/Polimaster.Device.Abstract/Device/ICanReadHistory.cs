using System.Collections.Generic;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device;

/// <summary>
/// Identifies a device with a history that can be read
/// </summary>
/// <typeparam name="THistory">Type of history record</typeparam>
/// <typeparam name="TParams"></typeparam>
public interface ICanReadHistory<THistory, in TParams> {
    Task<IEnumerable<THistory>> ReadHistory(TParams parameters);
}