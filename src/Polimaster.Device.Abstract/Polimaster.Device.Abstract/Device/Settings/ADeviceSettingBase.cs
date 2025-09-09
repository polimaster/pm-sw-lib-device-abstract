using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// <param name="settingDescriptor">See <see cref="ISettingDescriptor"/></param>
    protected ADeviceSettingBase(ISettingDescriptor settingDescriptor) {
        _isNullableValueType = Nullable.GetUnderlyingType(typeof(T)) != null || !typeof(T).IsValueType;
        if (settingDescriptor.ValueType != typeof(T))
            throw new Exception($"{nameof(settingDescriptor)} parameter should match type of {typeof(T)}, current is {settingDescriptor.ValueType}");

        Descriptor = settingDescriptor;
        ValidationErrors = [];
    }

    /// <inheritdoc />
    public Type ValueType => typeof(T);

    /// <summary>
    ///
    /// </summary>
    public object? UntypedValue {
        get => Value;
        set {
            switch (value) {
                case null:
                    Value = default;
                    return;
                case T typed:
                    Value = typed;
                    return;
                default:
                    throw new ArgumentException(
                        $"Invalid type '{value.GetType().Name}'. Expected '{typeof(T).Name}'.",
                        nameof(value)
                    );
            }
        }
    }

    /// <inheritdoc />
    public ISettingDescriptor Descriptor { get; }

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
                OnPropertyChanged(nameof(UntypedValue));
                OnPropertyChanged();
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

    /// <inheritdoc cref="HasValue" />
    private bool _hasValue;

    /// <inheritdoc />
    public virtual bool HasValue {
        get => _hasValue;
        protected set {
            if (value == _hasValue) return;
            _hasValue = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc cref="IsDirty" />
    private bool _isDirty;

    /// <inheritdoc />
    public virtual bool IsDirty {
        get => _isDirty;
        protected set {
            if (value == _isDirty) return;
            _isDirty = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc />
    public abstract bool IsSynchronized { get; }

    /// <inheritdoc />
    public virtual bool IsValid => !ValidationErrors.Any();

    /// <inheritdoc />
    public virtual bool IsError => Exception != null;

    /// <inheritdoc />
    public List<ValidationResult> ValidationErrors { get; }

    /// <inheritdoc cref="Exception" />
    private Exception? _exception;

    /// <inheritdoc />
    public virtual Exception? Exception {
        get => _exception;
        protected set {
            if (Equals(value, _exception)) return;
            _exception = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsError));
        }
    }

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
        OnPropertyChanged(nameof(ValidationErrors));
        OnPropertyChanged(nameof(IsValid));
    }

    /// <summary>
    /// Check if value is not null (for nullable and reference types)
    /// </summary>
    /// <param name="value">Value to check</param>
    /// <returns></returns>
    private bool ValueIsNotNull(T? value) {
        if (_isNullableValueType) return value is not null;
        return true;
    }

    /// <inheritdoc />
    public override string? ToString() {
        return Value != null ? Value.ToString() : null;
    }

    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Invoke <see cref="PropertyChanged"/>
    /// </summary>
    /// <param name="propertyName">name of property</param>
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}