﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Settings;

/// <summary>
/// Device setting base class
/// </summary>
/// <inheritdoc cref="IDeviceSetting{T}"/>
public class DeviceSettingBase<T> : ADeviceSetting<T> {
    /// <inheritdoc />
    public DeviceSettingBase(ITransport transport, IDataReader<T> reader, IDataWriter<T>? writer = null) : base(transport, reader, writer) {
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
    public override async Task Read(CancellationToken cancellationToken) {
        try {
            var v = await Transport.Read(Reader, cancellationToken);
            SetValue(v);
        } catch (Exception e) {
            SetValue(default);
            Exception = e;
        }
    }

    /// <inheritdoc />
    public override async Task CommitChanges(CancellationToken cancellationToken) {
        if (!IsValid) {
            Exception = new Exception($"Value of {GetType()} is not valid");
            return;
        }
        
        if (Writer == null || !IsDirty) return;
        try {
            await Transport.Write(Writer, Value, cancellationToken);
            IsDirty = false;
            Exception = null;
        } catch (Exception e) {
            Exception = e;
        }
    }
}