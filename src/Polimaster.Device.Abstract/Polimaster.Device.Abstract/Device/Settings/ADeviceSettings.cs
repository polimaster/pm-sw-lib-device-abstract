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
public abstract class ADeviceSettings<T> : IDeviceSetting<T>{
    public virtual ICommand<T>? ReadCommand { get; set; }
    public virtual ICommand<T>? WriteCommand { get; set; }
    public virtual T? Value { get; set; }
    public bool IsDirty { get; protected set; }

    public bool IsValid {
        get {
            if (ValidationErrors == null || !ValidationErrors.Any()) return true;
            return false;
        }
    }

    public bool IsError => Exception != null;
    public IEnumerable<SettingValidationException>? ValidationErrors { get; protected set; }
    public Exception? Exception { get; protected set; }
    public abstract Task Read(CancellationToken cancellationToken);

    public abstract Task CommitChanges(CancellationToken cancellationToken);
}