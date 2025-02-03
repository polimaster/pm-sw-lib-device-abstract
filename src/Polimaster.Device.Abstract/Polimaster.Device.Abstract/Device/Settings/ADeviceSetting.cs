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
public abstract class ADeviceSetting<T> : IDeviceSetting<T>{

    /// <summary>
    /// Stores type nullability for <see cref="T"/>
    /// </summary>
    private readonly bool _isNullableType;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="settingBehaviour">See <see cref="ISettingBehaviour"/></param>
    protected ADeviceSetting(ISettingBehaviour? settingBehaviour = null) {
        _isNullableType = Nullable.GetUnderlyingType(typeof(T)) != null || !typeof(T).IsValueType;
        Behaviour = settingBehaviour ?? new SettingBehaviourBase();
        ValidationErrors = [];
    }

    /// <inheritdoc />
    public ISettingBehaviour? Behaviour { get; }

    /// <inheritdoc />
    public abstract bool ReadOnly { get; }

    /// <inheritdoc />
    public abstract T? Value { get; set; }

    /// <inheritdoc />
    public bool HasValue => CheckIfNull(Value);

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
    public Exception? Exception { get; protected set; }

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
        var isNull = CheckIfNull(value);
        if (!isNull) ValidationErrors.Add(new ValidationResult("Value can't be null"));
    }

    /// <summary>
    /// Check if value is null (for nullable types) or default for value type
    /// </summary>
    /// <param name="value">Value to check</param>
    /// <returns></returns>
    protected bool CheckIfNull(T? value) {
        return _isNullableType ? value is not null : !EqualityComparer<T>.Default.Equals(value!, default!);
    }

    /// <inheritdoc />
    public override string? ToString() {
        return Value != null ? Value.ToString() : null;
    }
}