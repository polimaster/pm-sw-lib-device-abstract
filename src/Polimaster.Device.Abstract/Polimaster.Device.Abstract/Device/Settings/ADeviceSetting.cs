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
public abstract class ADeviceSetting<T> : IDeviceSetting<T> where T : notnull {
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="settingBehaviour">See <see cref="ISettingBehaviour"/></param>
    protected ADeviceSetting(ISettingBehaviour? settingBehaviour = null) {
        Behaviour = settingBehaviour ?? new SettingBehaviourBase();
    }

    /// <inheritdoc />
    public ISettingBehaviour? Behaviour { get; }

    /// <inheritdoc />
    public abstract bool ReadOnly { get; }

    /// <inheritdoc />
    public abstract T? Value { get; set; }

    /// <inheritdoc />
    public bool IsDirty { get; protected set; }

    /// <inheritdoc />
    public abstract bool IsSynchronized { get; protected set; }

    /// <inheritdoc />
    public bool IsValid => !ValidationErrors.Any();

    /// <inheritdoc />
    public bool IsError => Exception != null;

    /// <inheritdoc />
    public List<ValidationResult> ValidationErrors { get; protected set; } = [];

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
        ValidationErrors = [];
        if (value == null) {
            ValidationErrors = [
                new ValidationResult("Value is null",
                    new ArgumentNullException(nameof(value), $"{nameof(value)} cannot be null."))
            ];
        }
    }

    /// <inheritdoc />
    public override string? ToString() {
        return Value != null ? Value.ToString() : null;
    }
}