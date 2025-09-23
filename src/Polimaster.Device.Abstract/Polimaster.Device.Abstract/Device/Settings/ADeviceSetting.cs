using System;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device.Commands;

namespace Polimaster.Device.Abstract.Device.Settings;

/// <summary>
/// Device setting base class
/// </summary>
/// <inheritdoc cref="IDeviceSetting{T}"/>
public class ADeviceSetting<T> : ADeviceSettingBase<T> {
    /// <summary>
    /// Set limit of threads to 1, witch can access to read/write operations at a time. 
    /// </summary>
    protected SemaphoreSlim Semaphore { get; } = new(1, 1);

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="settingDefinition"></param>
    protected ADeviceSetting(SettingDefinition<T> settingDefinition) : base(settingDefinition.Descriptor) {
        Reader = settingDefinition.Reader ?? throw new NullReferenceException($"{nameof(settingDefinition.Reader)} is null");
        Writer = settingDefinition.Writer;
    }

    /// <summary>
    /// Command for read data
    /// </summary>
    protected IDataReader<T> Reader { get; }

    /// <summary>
    /// Command for write data
    /// </summary>
    protected IDataWriter<T>? Writer { get; }

    /// <inheritdoc />
    public override bool ReadOnly => Writer == null;

    /// <inheritdoc />
    public override bool IsSynchronized => _isSynchronized;

    /// <summary>
    ///
    /// </summary>
    private bool _isSynchronized;

    /// <inheritdoc />
    public override Task Read(CancellationToken cancellationToken) {
        return IsSynchronized ? Task.CompletedTask : Reset(cancellationToken);
    }

    /// <inheritdoc />
    public override async Task Reset(CancellationToken cancellationToken) {
        await Semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
        try {
            var v = await Reader.Read(cancellationToken);
            SetValue(v);
            _isSynchronized = true;
        } catch (Exception e) {
            _isSynchronized = false;
            SetValue(default);
            HasValue = false;
            Exception = e;
        } finally {
            OnPropertyChanged(nameof(IsSynchronized));
            if (Semaphore.CurrentCount < 1) Semaphore.Release();
        }
    }

    /// <inheritdoc />
    public override async Task CommitChanges(CancellationToken cancellationToken) {
        if (!IsValid) {
            Exception = new Exception("Value is not valid");
            return;
        }

        if (Writer == null || !IsDirty) return;

        try {
            await Semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
            if (Value is not null) await Writer.Write(Value, cancellationToken);
            IsDirty = false;
            Exception = null;
            _isSynchronized = true;
            OnPropertyChanged(nameof(IsSynchronized));
        } catch (Exception e) {
            Exception = e;
        } finally { if (Semaphore.CurrentCount < 1) Semaphore.Release(); }
    }
}