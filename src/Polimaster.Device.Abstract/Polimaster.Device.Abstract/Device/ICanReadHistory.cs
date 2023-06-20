using System.Collections.Generic;

namespace Polimaster.Device.Abstract.Device; 

/// <summary>
/// Identifies a device with a history that can be read
/// </summary>
/// <typeparam name="T">Type of history record</typeparam>
public interface ICanReadHistory<out T> {
    IEnumerable<T> ReadHistory();
}