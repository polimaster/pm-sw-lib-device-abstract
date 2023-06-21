using System;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Settings;

public abstract class ADeviceSetting<T> : IDeviceSetting<T> {
    public T? Value { get; private set; }
    public bool IsDirty { get; private set; }

    public bool IsError => Exception != null;
    public Exception? Exception { get; private set; }

    /// <summary>
    /// Sets <see cref="Value"/> red from device
    /// </summary>
    /// <param name="value">new value</param>
    protected void AcceptDeviceValue(T? value = default) {
        Value = value;
        IsDirty = false;
        Exception = null;
    }

    /// <summary>
    /// Sets error while device communication
    /// </summary>
    /// <param name="exception"></param>
    protected void SetError(Exception exception) {
        Exception = exception;
        IsDirty = false;
        Value = default;
    }
    
    public abstract Task Read();
    public abstract Task CommitChanges();
}