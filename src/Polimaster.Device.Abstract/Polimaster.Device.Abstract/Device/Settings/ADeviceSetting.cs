using System;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Settings;

public abstract class DeviceSettingBool : ADeviceSetting<bool?> { }
public abstract class DeviceSettingInt : ADeviceSetting<int?> { }
public abstract class DeviceSettingFloat : ADeviceSetting<float?> { }
public abstract class DeviceSettingDouble : ADeviceSetting<double?> { }
public abstract class DeviceSettingUshort : ADeviceSetting<ushort?> { }
public abstract class DeviceSettingString : ADeviceSetting<string?> { }

public abstract class ADeviceSetting<T> : IDeviceSetting<T> {
    private T? _value;

    public T? Value {
        get => _value;
        set {
            try {
                Validate(value);
                IsDirty = true;
                Exception = null;
                _value = value;
            } catch (Exception e) {
                SetError(e);
            }
        }
    }

    public bool IsDirty { get; protected set; }

    public bool IsError => Exception != null;
    public Exception? Exception { get; private set; }

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
    
    /// <summary>
    /// Validates value while assignment
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="SettingValidationException"></exception>
    protected virtual void Validate(T? value) { }
}