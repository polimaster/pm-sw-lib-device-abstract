﻿using System;
using Polimaster.Device.Abstract.Device.Settings;

namespace Polimaster.Device.Abstract.Device.Implementations.History;

/// <summary>
/// Identifies a device can return history
/// </summary>
/// <typeparam name="THistory">Type of history record</typeparam>
public interface IHasHistory<THistory> : IHasClock where THistory : AHistoryRecord {
    
    /// <summary>
    /// <see cref="IHistoryManager{THistory}"/>
    /// </summary>
    IHistoryManager<THistory> HistoryManager { get; }

    /// <summary>
    /// Interval between history entries
    /// </summary>
    IDeviceSetting<TimeSpan> HistoryInterval { get; }
}