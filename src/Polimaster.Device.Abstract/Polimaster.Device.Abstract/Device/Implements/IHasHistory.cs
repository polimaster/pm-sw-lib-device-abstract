using System.Collections.Generic;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Implements;

/// <summary>
/// Identifies a device can record history
/// </summary>
/// <typeparam name="THistory">Type of history record</typeparam>
public interface IHasHistory<THistory> : IHasClock {
    Task<IEnumerable<THistory>> ReadHistory<T>(T? parameters = default);

    Task WipeHistory<T>(T? parameters = default);
}