using System;
using System.Data;

namespace Polimaster.Device.Abstract.Transport;

/// <inheritdoc cref="ITransport{TData}"/>
public abstract class ATransport<TData> : ITransport<TData> {
    /// <inheritdoc cref="ITransport{TData}.ConnectionState"/>
    public ConnectionState ConnectionState { get; protected set; }

    /// <inheritdoc cref="ITransport{TData}.ConnectionStateChanged"/>
    public event Action<ConnectionState>? ConnectionStateChanged;

    /// <inheritdoc cref="ITransport{TData}.Write"/>
    public void Write(TData command) {
        if (ConnectionState != ConnectionState.Open) throw new TransportException();
    }

    /// <inheritdoc cref="ITransport{TData}.Read"/>
    public abstract TData Read(TData command);

    /// <inheritdoc cref="ITransport{TData}.Open"/>
    public virtual void Open() {
        if (ConnectionState == ConnectionState.Open) return;
        ConnectionState = ConnectionState.Open;
        ConnectionStateChanged?.Invoke(ConnectionState);
    }

    /// <inheritdoc cref="ITransport{TData}.Close"/>
    public virtual void Close() {
        if (ConnectionState == ConnectionState.Closed) return;
        ConnectionState = ConnectionState.Closed;
        ConnectionStateChanged?.Invoke(ConnectionState);
    }

    public void Dispose() {
        Close();
    }
}