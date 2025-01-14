using System;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device.Commands;

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
    
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="writer">Command for write data. If null it creates readonly setting.</param>
    /// <param name="reader">Command for read data</param>
    /// <param name="settingBehaviour">See <see cref="ISettingBehaviour"/></param>
    protected DeviceSettingBase(IDataReader<T?> reader, IDataWriter<T>? writer = null, ISettingBehaviour? settingBehaviour = null)
        : base(settingBehaviour) {
        Reader = reader;
        Writer = writer;
    }

    /// <summary>
    /// Command for read data
    /// </summary>
    protected IDataReader<T?> Reader { get; }

    /// <summary>
    /// Command for write data
    /// </summary>
    protected IDataWriter<T>? Writer { get; }

    /// <summary>
    /// <see cref="Value"/>
    /// </summary>
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
    public override bool ReadOnly => Writer == null;

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
    public override Task Read(CancellationToken cancellationToken) {
        return IsSynchronized ? Task.CompletedTask : Reset(cancellationToken);
    }

    /// <inheritdoc />
    public override async Task Reset(CancellationToken cancellationToken) {
        await Semaphore.WaitAsync(cancellationToken);
        try {
            var v = await Reader.Read(cancellationToken);
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
    public override async Task CommitChanges(CancellationToken cancellationToken) {
        if (!IsValid) {
            Exception = new Exception($"Value of {GetType().Name} is not valid");
            return;
        }
        
        if (Writer == null || !IsDirty || Value == null) return;
        await Semaphore.WaitAsync(cancellationToken);
        try {
            await Writer.Write(Value, cancellationToken);
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