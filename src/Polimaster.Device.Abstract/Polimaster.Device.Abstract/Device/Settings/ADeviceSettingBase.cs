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
public abstract class ADeviceSettingBase<T> : IDeviceSetting<T> {
    /// <summary>
    /// Synchronization object used to ensure thread safety when accessing or modifying values.
    /// </summary>
    private readonly object _valueLock = new();

    /// <summary>
    /// Internal value.
    /// </summary>
    private T? _internalValue;

    /// <summary>
    /// Indicates whether the current setting has a defined value.
    /// Used internally to track whether the setting has been initialized or updated.
    /// </summary>
    private bool _hasValue;

    /// <summary>
    /// Indicates whether the setting has been modified since it was last read.
    /// </summary>
    private bool _isDirty;

    /// <summary>
    /// Stores <see cref="Exception"/> occured while reading data from the device.
    /// </summary>
    private Exception? _exception;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="settingDescriptor">See <see cref="ISettingDescriptor"/></param>
    protected ADeviceSettingBase(ISettingDescriptor settingDescriptor) {

        if (!typeof(T).IsAssignableFrom(settingDescriptor.ValueType)) {
            throw new ArgumentException(
                $"{nameof(settingDescriptor.ValueType)} of {nameof(settingDescriptor)} parameter should be assignable to {typeof(T)}, " +
                $"current is {settingDescriptor.ValueType}");
        }

        Descriptor = settingDescriptor;
        ValidationResults = [];
    }

    /// <inheritdoc />
    public Type ValueType => typeof(T);

    /// <summary>
    /// Represents a generic untyped value that can be accessed or modified as an object.
    /// The property facilitates interaction with the stored value without requiring knowledge
    /// of its specific type, while ensuring type safety through validation during assignment.
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

    /// <inheritdoc />
    [Required]
    public virtual T? Value {
        get { lock (_valueLock) return _internalValue; }
        set {
            List<string> changed;
            bool valueChanged;
            lock (_valueLock) {
                changed = [];
                // user assignment is always dirty and clears error
                ApplyIsDirty(true, changed);
                ApplyException(null, changed);
                ApplyHasValue(value is not null, changed);
                _internalValue = value;
                changed.Add(nameof(Value));
                changed.Add(nameof(UntypedValue));
                valueChanged = true;
            }
            FireChanged(changed);
            if (valueChanged) Validate();
        }
    }

    /// <summary>
    /// Set <see cref="Value"/> from internal Read/Write commands.
    /// Resets <see cref="IsDirty"/> and <see cref="Exception"/>.
    /// </summary>
    protected void SetValue(T? value, bool isDirty = false) {
        List<string> changed;
        bool valueChanged;
        lock (_valueLock) {
            changed = [];
            ApplyIsDirty(isDirty, changed);
            ApplyException(null, changed);
            ApplyHasValue(value is not null, changed);
            // skip update if the value is unchanged and not a user-initiated dirty write
            if (!isDirty && value is not null && EqualityComparer<T>.Default.Equals(value, _internalValue)) {
                valueChanged = false;
            } else {
                _internalValue = value;
                changed.Add(nameof(Value));
                changed.Add(nameof(UntypedValue));
                valueChanged = true;
            }
        }
        FireChanged(changed);
        if (valueChanged) Validate();
    }

    /// <summary>
    /// Reset setting to error state.
    /// </summary>
    protected void SetError(Exception? exception) {
        List<string> changed;
        lock (_valueLock) {
            changed = [];
            ApplyHasValue(false, changed);
            if (_internalValue is not null) {
                _internalValue = default;
                changed.Add(nameof(Value));
                changed.Add(nameof(UntypedValue));
            }
            ApplyIsDirty(false, changed);
            ApplyException(exception, changed);
        }
        FireChanged(changed);
    }

    // --- Helpers: must be called under _valueLock ---

    /// <summary>
    /// Updates the internal state to reflect whether the setting has a valid value.
    /// </summary>
    /// <param name="value">A boolean indicating whether the setting has a value.</param>
    /// <param name="changed">A list of property names that will be updated when the state changes.</param>
    private void ApplyHasValue(bool value, List<string> changed) {
        if (_hasValue == value) return;
        _hasValue = value;
        changed.Add(nameof(HasValue));
    }

    /// <summary>
    /// Updates the internal dirty state of the setting and adds the "IsDirty" property name
    /// to the list of changed properties if the dirty state is altered.
    /// </summary>
    /// <param name="value">The new value indicating whether the setting is dirty.</param>
    /// <param name="changed">A list to track the names of properties that have changed.</param>
    private void ApplyIsDirty(bool value, List<string> changed) {
        if (_isDirty == value) return;
        _isDirty = value;
        changed.Add(nameof(IsDirty));
    }

    /// <summary>
    /// Updates the exception state of the setting and tracks the changed properties.
    /// </summary>
    /// <param name="value">The exception to set. Can be null to clear the current error state.</param>
    /// <param name="changed">A list of property names that have changed as a result of this operation.</param>
    private void ApplyException(Exception? value, List<string> changed) {
        if (Equals(_exception, value)) return;
        _exception = value;
        changed.Add(nameof(Exception));
        changed.Add(nameof(IsError));
    }

    /// <summary>
    /// Fire PropertyChanged event for each property in the list.
    /// </summary>
    /// <param name="changed">List of property names that have changed.</param>
    private void FireChanged(List<string> changed) {
        foreach (var name in changed) OnPropertyChanged(name);
    }

    // --- Public properties ---

    /// <inheritdoc />
    public virtual bool HasValue {
        get => _hasValue;
        protected set {
            List<string> changed = [];
            lock (_valueLock) { ApplyHasValue(value, changed); }
            FireChanged(changed);
        }
    }

    /// <inheritdoc />
    public virtual bool IsDirty {
        get => _isDirty;
        protected set {
            List<string> changed = [];
            lock (_valueLock) { ApplyIsDirty(value, changed); }
            FireChanged(changed);
        }
    }

    /// <inheritdoc />
    public abstract bool IsSynchronized { get; }

    /// <inheritdoc />
    public virtual bool IsValid => !ValidationResults.Any();

    /// <inheritdoc />
    public virtual bool IsError => _exception != null;

    /// <inheritdoc />
    public List<ValidationResult> ValidationResults { get; }

    /// <inheritdoc />
    public virtual Exception? Exception {
        get => _exception;
        protected set {
            List<string> changed = [];
            lock (_valueLock) { ApplyException(value, changed); }
            FireChanged(changed);
        }
    }

    /// <inheritdoc />
    public abstract Task Read(CancellationToken cancellationToken);

    /// <inheritdoc />
    public abstract Task Reset(CancellationToken cancellationToken);

    /// <inheritdoc />
    public abstract Task CommitChanges(CancellationToken cancellationToken);

    /// <inheritdoc />
    public virtual void Dispose() { }

    /// <summary>
    /// Validates value while assignment. See <see cref="ValidationResults"/> for errors.
    /// Validation work is done outside the lock; results are applied atomically.
    /// </summary>
    protected virtual void Validate() {
        var snapshot = Value;
        var results = new List<ValidationResult>();

        var memberContext = new ValidationContext(this) { MemberName = nameof(Value) };
        Validator.TryValidateProperty(snapshot, memberContext, results);

        if (snapshot is not null) {
            var type = snapshot.GetType();
            if (!type.IsSimpleType()) { // if value is complex object
                var valueContext = new ValidationContext(snapshot);
                Validator.TryValidateObject(snapshot, valueContext, results, validateAllProperties: true);
            }
        }

        lock (_valueLock) {
            ValidationResults.Clear();
            ValidationResults.AddRange(results);
        }

        OnPropertyChanged(nameof(ValidationResults));
        OnPropertyChanged(nameof(IsValid));
    }

    /// <summary>
    /// Appends additional validation results atomically.
    /// Used by subclasses to merge validation errors from proxied settings.
    /// </summary>
    protected void AddValidationResults(IEnumerable<ValidationResult> additional) {
        lock (_valueLock) {
            ValidationResults.AddRange(additional);
        }
        OnPropertyChanged(nameof(ValidationResults));
        OnPropertyChanged(nameof(IsValid));
    }

    /// <inheritdoc />
    public override string? ToString() {
        if (ValueType == typeof(bool)) {
            return Value is true ? OnOff.ON.GetDescription() : OnOff.OFF.GetDescription();
        }
        if (ValueType.IsEnum) {
            var v =  Value as Enum;
            return v?.GetDescription();
        }

        return Value?.ToString();
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