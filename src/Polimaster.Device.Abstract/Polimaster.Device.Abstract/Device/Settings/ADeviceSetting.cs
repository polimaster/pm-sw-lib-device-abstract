using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device.Commands.Interfaces;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;

namespace Polimaster.Device.Abstract.Device.Settings;

/// <summary>
/// <see cref="IDeviceSetting{T}"/> abstract implementation
/// </summary>
/// <typeparam name="T"><inheritdoc cref="IDeviceSetting{T}"/></typeparam>
public abstract class ADeviceSetting<T> : IDeviceSetting<T>{
    /// <inheritdoc />
    public virtual ICommand<T>? ReadCommand { get; set; }

    /// <inheritdoc />
    public virtual ICommand<T>? WriteCommand { get; set; }

    /// <inheritdoc />
    public bool ReadOnly => WriteCommand == null;

    /// <inheritdoc />
    public virtual T? Value { get; set; }

    /// <inheritdoc />
    public bool IsDirty { get; protected set; }

    /// <inheritdoc />
    public bool IsValid => ValidationErrors == null || !ValidationErrors.Any();

    /// <inheritdoc />
    public bool IsError => Exception != null;

    /// <inheritdoc />
    public IEnumerable<SettingValidationException>? ValidationErrors { get; protected set; }

    /// <inheritdoc />
    public Exception? Exception { get; protected set; }

    /// <inheritdoc />
    public abstract Task Read(CancellationToken cancellationToken);

    /// <inheritdoc />
    public abstract Task CommitChanges(CancellationToken cancellationToken);

    /// <inheritdoc />
    public override string? ToString() {
        return Value != null ? Value.ToString() : null;
    }
}