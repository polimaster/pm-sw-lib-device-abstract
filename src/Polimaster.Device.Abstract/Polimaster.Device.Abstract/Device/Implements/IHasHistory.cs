using System.Collections.Generic;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Implements;

/// <summary>
/// Identifies a device can return history
/// </summary>
/// <typeparam name="THistory">Type of history record</typeparam>
public interface IHasHistory<THistory> : IHasClock {
    Task<IEnumerable<THistory>> ReadHistory();

    Task WipeHistory();
}

/// <summary>
/// Identifies a device can return history with parameters
/// </summary>
/// <typeparam name="THistory">Type of history record</typeparam>
/// <typeparam name="TReadParams">History reading parameters</typeparam>
public interface IHasHistory<THistory, in TReadParams> : IHasHistory<THistory> {
    Task<IEnumerable<THistory>> ReadHistory(TReadParams? parameters = default);
}