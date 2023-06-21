using System;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Settings;

public interface IDeviceSetting<out T> {
    T? Value { get; }
    bool IsDirty { get; }
    bool IsError { get; }
    Exception? Exception { get; }

    Task Read();
    Task CommitChanges();
}