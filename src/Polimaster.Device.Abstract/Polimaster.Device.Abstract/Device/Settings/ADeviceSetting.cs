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
    /// Constructor
    /// </summary>
    /// <param name="settingDefinition"></param>
    protected ADeviceSetting(SettingDefinition<T> settingDefinition) : base(settingDefinition.Descriptor) {
        Reader = settingDefinition.Reader;
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
    /// <see cref="IsSynchronized"/>
    /// </summary>
    private bool _isSynchronized;

    /// <inheritdoc />
    public override Task Read(CancellationToken cancellationToken) {
        return IsSynchronized ? Task.CompletedTask : Reset(cancellationToken);
    }

    /// <inheritdoc />
    public override async Task Reset(CancellationToken cancellationToken) {
        try {
            var v = await Reader.Read(cancellationToken);
            SetValue(v);
            _isSynchronized = true;
        } catch (Exception e) {
            _isSynchronized = false;
            SetError(e);
        } finally {
            OnPropertyChanged(nameof(IsSynchronized));
        }
    }

    /// <inheritdoc />
    public override async Task CommitChanges(CancellationToken cancellationToken) {
        if (!IsValid) {
            Exception = new InvalidOperationException("Value is not valid");
            return;
        }

        if (Writer == null || !IsDirty) return;

        try {
            if (Value is not null) await Writer.Write(Value, cancellationToken);
            IsDirty = false;
            Exception = null;
            _isSynchronized = true;
            OnPropertyChanged(nameof(IsSynchronized));
        } catch (Exception e) {
            Exception = e;
        }
    }
}