using System;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Commands;

namespace Polimaster.Device.Abstract.Device.Settings;

public class DeviceSettingBase<T, TData> : IDeviceSetting<T> {
    private readonly ICommand<T, TData> _readCommand;
    private readonly ICommand<T, TData>? _writeCommand;
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

    public DeviceSettingBase(ICommand<T, TData> readCommand, ICommand<T, TData>? writeCommand = null) {
        _readCommand = readCommand;
        _writeCommand = writeCommand;
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

    public virtual async Task Read(CancellationToken cancellationToken) {
        try {
            await _readCommand.Send(cancellationToken);
            Value = _readCommand.Value;
            IsDirty = false;
        } catch (Exception e) { SetError(e); }
    }

    public virtual async Task CommitChanges(CancellationToken cancellationToken) {
        if (!IsDirty) return;
        try {
            if (_writeCommand != null) {
                _writeCommand.Value = Value;
                await _writeCommand.Send(cancellationToken);
            }
        } catch (Exception e) { SetError(e); }
    }
    
    /// <summary>
    /// Validates value while assignment
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="SettingValidationException"></exception>
    protected virtual void Validate(T? value) { }
}