using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Settings;

/// <summary>
/// <see cref="IDeviceSetting{T}"/> abstract implementation
/// </summary>
/// <typeparam name="T"><inheritdoc cref="IDeviceSetting{T}"/></typeparam>
/// <typeparam name="TStream"></typeparam>
public abstract class ADeviceSetting<T, TStream> : IDeviceSetting<T>{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="transport"></param>
    /// <param name="readCommand">Command for read data</param>
    /// <param name="writeCommand">Command for write data. If null it creates readonly setting.</param>
    protected ADeviceSetting(ITransport<TStream> transport, ICommand<T, TStream> readCommand, ICommand<T, TStream>? writeCommand = null) {
        Transport = transport;
        ReadCommand = readCommand;
        WriteCommand = writeCommand;
    }

    /// <see cref="ITransport"/>
    protected ITransport<TStream> Transport { get; }

    /// <summary>
    /// Command for read data
    /// </summary>
    protected ICommand<T, TStream> ReadCommand { get; }

    /// <summary>
    /// Command for write data
    /// </summary>
    protected ICommand<T, TStream>? WriteCommand { get; }

    /// <inheritdoc />
    public bool ReadOnly => WriteCommand == null;

    /// <inheritdoc />
    public abstract T? Value { get; set; }

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