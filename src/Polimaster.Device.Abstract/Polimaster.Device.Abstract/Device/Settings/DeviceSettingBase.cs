using System;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Settings;

/// <summary>
/// Device setting base class
/// </summary>
/// <inheritdoc cref="IDeviceSetting{T}"/>
public class DeviceSettingBase<T> : ADeviceSetting<T> {
    /// <summary>
    /// Set limit of threads to 1, witch can access to read/write operations at a time. 
    /// </summary>
    protected SemaphoreSlim Semaphore { get; } = new(1, 1);
    
    /// <inheritdoc />
    protected DeviceSettingBase(IDataReader<T> reader, IDataWriter<T>? writer = null) : base(reader, writer) {
    }

    private T? _value;

    /// <inheritdoc />
    public override T? Value {
        get => _value;
        set {
            Validate(value);
            SetValue(value);
            IsDirty = true;
        }
    }

    /// <inheritdoc />
    public override bool IsSynchronized { get; protected set; }

    /// <summary>
    /// Set value from internal Read/Write commands
    /// </summary>
    /// <param name="value"></param>
    private void SetValue(T? value) {
        IsDirty = false;
        Exception = null;
        _value = value;
    }

    /// <inheritdoc />
    public override async Task Read(ITransport transport, CancellationToken cancellationToken) {
        await Semaphore.WaitAsync(cancellationToken);
        try {
            if(IsSynchronized && !IsDirty) return;
            var v = await transport.Read(Reader, cancellationToken);
            SetValue(v);
            IsSynchronized = true;
        } catch (Exception e) {
            IsSynchronized = false;
            SetValue(default);
            Exception = e;
        } finally {
            Semaphore.Release();
        }
    }

    /// <inheritdoc />
    public override async Task CommitChanges(ITransport transport, CancellationToken cancellationToken) {
        if (!IsValid) {
            Exception = new Exception($"Value of {GetType().Name} is not valid");
            return;
        }
        
        if (Writer == null || !IsDirty) return;
        await Semaphore.WaitAsync(cancellationToken);
        try {
            await transport.Write(Writer!, Value, cancellationToken);
            IsDirty = false;
            Exception = null;
            IsSynchronized = true;
        } catch (Exception e) {
            Exception = e;
        } finally {
            Semaphore.Release();
        }
    }
}