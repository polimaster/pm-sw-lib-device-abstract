using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Helpers;

namespace Polimaster.Device.Abstract.Device.Settings;

/// <summary>
/// <see cref="IDeviceSetting{T}"/> abstract implementation
/// </summary>
/// <typeparam name="T"><inheritdoc cref="IDeviceSetting{T}"/></typeparam>
public abstract class ADeviceSettingBase<T> : IDeviceSetting<T> where T : notnull {
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="settingDescriptor">See <see cref="ISettingDescriptor"/></param>
    protected ADeviceSettingBase(ISettingDescriptor settingDescriptor) {

        if (!settingDescriptor.ValueType.IsAssignableFrom(typeof(T))) {
            throw new Exception(
                $"{nameof(settingDescriptor.ValueType)} of {nameof(settingDescriptor)} parameter should be assignable from {typeof(T)}, " +
                $"current is {settingDescriptor.ValueType}");
        }

        Descriptor = settingDescriptor;
        ValidationResults = [];
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
    [Required]
    public virtual T? Value {
        get => _internalValue;
        set {
            lock (this) {
                SetValue(value, true);
                Validate();
            }
        }
    }

    /// <summary>
    /// Set <see cref="Value"/> from internal Read/Write commands.
    /// Note it resets <see cref="IsDirty"/> and <see cref="Exception"/> fields.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="isDirty"></param>
    protected void SetValue(T? value, bool isDirty = false) {
        IsDirty = isDirty;
        Exception = null;
        _internalValue = value;
        HasValue = true;
        OnPropertyChanged(nameof(Value));
        OnPropertyChanged(nameof(UntypedValue));
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
    public virtual bool IsValid => !ValidationResults.Any();

    /// <inheritdoc />
    public virtual bool IsError => Exception != null;

    /// <inheritdoc />
    public List<ValidationResult> ValidationResults { get; }

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
    /// Validates value while assignment. See <see cref="ValidationResults"/> for errors.
    /// </summary>
    protected virtual void Validate() {
        ValidationResults.Clear();

        var memberContext = new ValidationContext(this) { MemberName = nameof(Value) };
        Validator.TryValidateProperty(Value, memberContext, ValidationResults);

        if (Value is not null) {
            var type = Value.GetType();
            if (!type.IsSimpleType()) { // if value is complex object
                var valueContext = new ValidationContext(Value);
                Validator.TryValidateObject(Value, valueContext, ValidationResults, validateAllProperties: true);
            }
        }

        OnPropertyChanged(nameof(ValidationResults));
        OnPropertyChanged(nameof(IsValid));
    }

    /// <inheritdoc />
    public override string? ToString() {
        return Value is not null ? Value.ToString() : null;
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