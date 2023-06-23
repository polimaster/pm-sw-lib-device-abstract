﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Implements;

/// <summary>
/// Identifies a device can return history
/// </summary>
/// <typeparam name="THistory">Type of history record</typeparam>
public interface IHasHistory<THistory> : IHasClock {
    /// <summary>
    /// Reads history from device
    /// </summary>
    /// <returns>Array of history records</returns>
    Task<IEnumerable<THistory>> ReadHistory();

    /// <summary>
    /// Wipe history from device
    /// </summary>
    /// <returns></returns>
    Task WipeHistory();
}

/// <summary>
/// Identifies a device can return history with parameters
/// </summary>
/// <typeparam name="THistory">Type of history record</typeparam>
/// <typeparam name="TReadParams">History reading parameters</typeparam>
public interface IHasHistory<THistory, in TReadParams> : IHasHistory<THistory> {
    /// <summary>
    /// Reads history from device
    /// </summary>
    /// <param name="parameters">Parameters while reading history</param>
    /// <returns>Array of history records</returns>
    Task<IEnumerable<THistory>> ReadHistory(TReadParams? parameters = default);
}