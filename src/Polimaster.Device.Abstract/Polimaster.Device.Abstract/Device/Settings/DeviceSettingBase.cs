using System;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Commands;

namespace Polimaster.Device.Abstract.Device.Settings;

/// <summary>
/// Device setting base class
/// </summary>
/// <typeparam name="T"><see cref="IDeviceSetting{T}.Value"/> type</typeparam>
/// <typeparam name="TData">Transport data type for <see cref="ICommand{TValue,TTransportData}"/></typeparam>
public class DeviceSettingBase<T, TData> : IDeviceSetting<T> {
    protected readonly ICommand<T, TData>? ReadCommand;
    protected readonly ICommand<T, TData>? WriteCommand;
    private T? _value;

    public virtual T? Value {
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

    public DeviceSettingBase(ICommand<T, TData>? readCommand = null, ICommand<T, TData>? writeCommand = null) {
        ReadCommand = readCommand;
        WriteCommand = writeCommand;
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
            if (ReadCommand != null) {
                await ReadCommand.Send(cancellationToken);
                Value = ReadCommand.Value;
            }
            IsDirty = false;
        } catch (Exception e) { SetError(e); }
    }

    public virtual async Task CommitChanges(CancellationToken cancellationToken) {
        if (!IsDirty) return;
        try {
            if (WriteCommand != null) {
                WriteCommand.Value = Value;
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