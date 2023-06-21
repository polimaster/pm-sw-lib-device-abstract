using System;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Implements; 

/// <summary>
/// Identifies a device with internal clock
/// </summary>
public interface IHasClock {
    Task SetTime(DateTime? dateTime = default);
    Task<DateTime> GetTime();
}