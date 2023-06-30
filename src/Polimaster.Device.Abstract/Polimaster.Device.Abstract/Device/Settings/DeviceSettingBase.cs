using System;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device.Commands.Interfaces;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;

namespace Polimaster.Device.Abstract.Device.Settings;

/// <summary>
/// Device setting base class
/// </summary>
/// <typeparam name="T"><see cref="IDeviceSetting{T}.Value"/> type</typeparam>
public class DeviceSettingBase<T> : ADeviceSettings<T>, IDeviceSetting<T> {

    private T? _value;
    private ICommand<T>? _readCommand;
    private ICommand<T>? _writeCommand;

    public override ICommand<T>? ReadCommand {
        get => _readCommand;
        set {
            _readCommand = value;
            if (_readCommand != null) _readCommand.ValueChanged += v => _value = v;
        }
    }

    public override ICommand<T>? WriteCommand {
        get => _writeCommand;
        set {
            _writeCommand = value;
            if (_writeCommand != null) _writeCommand.ValueChanged += v => _value = v;
        }
    }

    public override T? Value {
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

    /// <summary>
    /// Sets error while device communication
    /// </summary>
    /// <param name="exception"></param>
    protected void SetError(Exception exception) {
        Exception = exception;
        IsDirty = false;
        Value = default;
    }

    public override async Task Read(CancellationToken cancellationToken) {
        try {
            if (ReadCommand != null) {
                await ReadCommand.Send(cancellationToken);
            }
            IsDirty = false;
        } catch (Exception e) { SetError(e); }
    }

    public override async Task CommitChanges(CancellationToken cancellationToken) {
        if (!IsDirty) return;
        try {
            if (WriteCommand != null) {
                await WriteCommand.Send(cancellationToken);
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