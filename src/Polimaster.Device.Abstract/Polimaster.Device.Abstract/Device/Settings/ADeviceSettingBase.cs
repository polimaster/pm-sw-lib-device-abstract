using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Settings;

/// <summary>
/// <see cref="IDeviceSetting{T}"/> abstract implementation
/// </summary>
/// <typeparam name="T"><inheritdoc cref="IDeviceSetting{T}"/></typeparam>
public abstract class ADeviceSettingBase<T> : IDeviceSetting<T> where T : notnull {

    /// <summary>
    /// Stores type nullability for <see cref="T"/>
    /// </summary>
    private readonly bool _isNullableValueType;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="settingBehaviour">See <see cref="ISettingBehaviour"/></param>
    protected ADeviceSettingBase(ISettingBehaviour? settingBehaviour = null) {
        _isNullableValueType = Nullable.GetUnderlyingType(typeof(T)) != null || !typeof(T).IsValueType;
        Behaviour = settingBehaviour ?? new SettingBehaviourBase();
        ValidationErrors = [];
    }

    /// <inheritdoc />
    public ISettingBehaviour? Behaviour { get; }

    /// <inheritdoc />
    public abstract bool ReadOnly { get; }

    /// <summary>
    /// Internal <see cref="Value"/>
    /// </summary>
    private T? _internalValue;

    /// <inheritdoc />
    public virtual T? Value {
        get => _internalValue;
        set {
            lock (this) {
                Validate(value);
                SetValue(value);
                IsDirty = true;
            }
        }
    }

    /// <summary>
    /// Set <see cref="Value"/> from internal Read/Write commands.
    /// Note it resets <see cref="IsDirty"/> and <see cref="Exception"/> fields.
    /// </summary>
    /// <param name="value"></param>
    protected void SetValue(T? value) {
        IsDirty = false;
        Exception = null;
        _internalValue = value;
        HasValue = true;
    }

    /// <inheritdoc />
    public virtual bool HasValue { get; protected set; }

    /// <inheritdoc />
    public virtual bool IsDirty { get; protected set; }

    /// <inheritdoc />
    public abstract bool IsSynchronized { get; }

    /// <inheritdoc />
    public virtual bool IsValid => !ValidationErrors.Any();

    /// <inheritdoc />
    public virtual bool IsError => Exception != null;

    /// <inheritdoc />
    public List<ValidationResult> ValidationErrors { get; }

    /// <inheritdoc />
    public virtual Exception? Exception { get; protected set; }

    /// <inheritdoc />
    public abstract Task Read(CancellationToken cancellationToken);

    /// <inheritdoc />
    public abstract Task Reset(CancellationToken cancellationToken);

    /// <inheritdoc />
    public abstract Task CommitChanges(CancellationToken cancellationToken);
    
    /// <summary>
    /// Validates value while assignment. See <see cref="ValidationErrors"/> for errors.
    /// </summary>
    /// <param name="value"><see cref="IDeviceSetting{T}.Value"/></param>
    protected virtual void Validate(T? value) {
        ValidationErrors.Clear();
        var notNull = ValueIsNotNull(value);
        if (!notNull) ValidationErrors.Add(new ValidationResult("Value can't be null"));
    }

    /// <summary>
    /// Check if value is not null (for nullable and reference types)
    /// </summary>
    /// <param name="value">Value to check</param>
    /// <returns></returns>
    private bool ValueIsNotNull(T? value) {
        if(_isNullableValueType) return value is not null;
        return true;
    }

    /// <inheritdoc />
    public override string? ToString() {
        return Value != null ? Value.ToString() : null;
    }
}