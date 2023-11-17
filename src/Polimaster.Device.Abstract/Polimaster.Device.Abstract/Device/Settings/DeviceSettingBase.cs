using System;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Settings;

/// <summary>
/// Device setting base class
/// </summary>
/// <inheritdoc cref="IDeviceSetting{T}"/>
public class DeviceSettingBase<T> : ADeviceSetting<T> {
    /// <inheritdoc />
    public DeviceSettingBase(ITransport transport, ICommand<T> readCommand, ICommand<T>? writeCommand = null) : base(transport, readCommand, writeCommand) {
    }

    private T? _value;

    /// <inheritdoc />
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
    private void SetValue(T? value) {
        IsDirty = false;
        Exception = null;
        _value = value;
    }

    /// <inheritdoc />
    public override async Task Read(CancellationToken cancellationToken) {
        try {
            await Transport.Exec(ReadCommand, TODO, cancellationToken);
        } catch (Exception e) {
            SetValue(default);
            Exception = e;
        }
    }

    /// <inheritdoc />
    public override async Task CommitChanges(CancellationToken cancellationToken) {
        if (!IsDirty || !IsValid) return;
        try {
            if (WriteCommand != null) {
                WriteCommand.Value = Value;
                await Transport.Exec(WriteCommand, TODO, cancellationToken);
            }
        } catch (Exception e) {
            Exception = e;
        }
    }

    /// <summary>
    /// Validates value while assignment.
    /// </summary>
    /// <param name="value"><see cref="IDeviceSetting{T}.Value"/></param>
    /// <exception cref="SettingValidationException">Throws if validation failed.</exception>
    protected virtual void Validate(T? value) {
        if (value == null) {
            
        }
    }
}