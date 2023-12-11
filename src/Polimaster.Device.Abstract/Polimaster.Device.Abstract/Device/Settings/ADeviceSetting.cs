using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Settings;

/// <summary>
/// <see cref="IDeviceSetting{T}"/> abstract implementation
/// </summary>
/// <typeparam name="T"><inheritdoc cref="IDeviceSetting{T}"/></typeparam>
public abstract class ADeviceSetting<T> : IDeviceSetting<T>{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="reader">Command for read data</param>
    /// <param name="writer">Command for write data. If null it creates readonly setting.</param>
    protected ADeviceSetting(IDataReader<T> reader, IDataWriter<T>? writer = null) {
        Reader = reader;
        Writer = writer;
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
    public bool ReadOnly => Writer == null;

    /// <inheritdoc />
    public abstract T? Value { get; set; }

    /// <inheritdoc />
    public bool IsDirty { get; protected set; }

    /// <inheritdoc />
    public abstract bool IsSynchronized { get; protected set; }

    /// <inheritdoc />
    public bool IsValid => ValidationErrors == null || !ValidationErrors.Any();

    /// <inheritdoc />
    public bool IsError => Exception != null;

    /// <inheritdoc />
    public IEnumerable<ValidationResult>? ValidationErrors { get; protected set; }

    /// <inheritdoc />
    public Exception? Exception { get; protected set; }

    /// <inheritdoc />
    public abstract Task Read(ITransport transport, CancellationToken cancellationToken);

    /// <inheritdoc />
    public abstract Task Reset(ITransport transport, CancellationToken cancellationToken);

    /// <inheritdoc />
    public abstract Task CommitChanges(ITransport transport, CancellationToken cancellationToken);
    
    /// <summary>
    /// Validates value while assignment. See <see cref="ValidationErrors"/> for errors.
    /// </summary>
    /// <param name="value"><see cref="IDeviceSetting{T}.Value"/></param>
    protected virtual void Validate(T? value) {
        ValidationErrors = null;
    }

    /// <inheritdoc />
    public override string? ToString() {
        return Value != null ? Value.ToString() : null;
    }
}