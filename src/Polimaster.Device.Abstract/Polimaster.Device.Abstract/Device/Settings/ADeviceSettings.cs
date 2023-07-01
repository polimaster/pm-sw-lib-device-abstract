using System;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device.Commands.Interfaces;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;

namespace Polimaster.Device.Abstract.Device.Settings;

public abstract class ADeviceSettings<T> : IDeviceSetting<T>{
    public virtual ICommand<T>? ReadCommand { get; set; }
    public virtual ICommand<T>? WriteCommand { get; set; }
    public virtual T? Value { get; set; }
    public bool IsDirty { get; protected set; }
    public bool IsError => Exception != null;
    public Exception? Exception { get; protected set; }
    public abstract Task Read(CancellationToken cancellationToken);

    public abstract Task CommitChanges(CancellationToken cancellationToken);
}