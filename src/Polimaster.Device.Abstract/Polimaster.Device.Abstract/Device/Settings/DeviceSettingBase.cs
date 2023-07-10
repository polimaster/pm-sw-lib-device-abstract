using System;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device.Commands.Interfaces;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;

namespace Polimaster.Device.Abstract.Device.Settings;

/// <summary>
/// Device setting base class
/// </summary>
/// <inheritdoc cref="IDeviceSetting{T}"/>
public class DeviceSettingBase<T> : ADeviceSetting<T> {

    private T? _value;
    private ICommand<T>? _readCommand;
    private ICommand<T>? _writeCommand;

    public override ICommand<T>? ReadCommand {
        get => _readCommand;
        set {
            _readCommand = value;
            if (_readCommand == null) return;
            SetValue(_readCommand.Value);
            _readCommand.ValueChanged += SetValue;
        }
    }

    public override ICommand<T>? WriteCommand {
        get => _writeCommand;
        set {
            _writeCommand = value;
            if (_writeCommand == null) return;
            SetValue(_writeCommand.Value);
            _writeCommand.ValueChanged += SetValue;
        }
    }

    public override T? Value {
        get => _value;
        set {
            try {
                Validate(value);
                SetValue(value);
                IsDirty = true;
            } catch (Exception e) {
                Exception = e;
            }
        }
    }

    /// <summary>
    /// Set value from internal Read/Write commands
    /// </summary>
    /// <param name="value"></param>
    protected void SetValue(T? value) {
        IsDirty = false;
        Exception = null;
        _value = value;
    }

    public override async Task Read(CancellationToken cancellationToken) {
        try {
            if (ReadCommand != null) await ReadCommand.Send(cancellationToken);
        } catch (Exception e) {
            SetValue(default);
            Exception = e;
        }
    }

    public override async Task CommitChanges(CancellationToken cancellationToken) {
        if (!IsDirty || !IsValid) return;
        try {
            if (WriteCommand != null) await WriteCommand.Send(cancellationToken);
        } catch (Exception e) {
            Exception = e;
        }
    }
    
    /// <summary>
    /// Validates value while assignment.
    /// </summary>
    /// <param name="value"><see cref="IDeviceSetting{T}.Value"/></param>
    /// <exception cref="SettingValidationException">Throws if validation failed.</exception>
    protected virtual void Validate(T? value) { }
}