using System;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device.Commands;

namespace Polimaster.Device.Abstract.Transport;

/// <summary>
/// Device transport layer (USB, Tcp, Bluetooth etc)
/// </summary>
public interface ITransport : IDisposable, IEquatable<ITransport> {
    /// <summary>
    /// Connection identifier
    /// </summary>
    string ConnectionId { get; }

    /// <summary>
    /// Indicates connection will be closed
    /// </summary>
    public event Action? Closing;

    /// <summary>
    /// Open device connection
    /// </summary>
    /// <returns></returns>
    Task OpenAsync();

    /// <summary>
    /// Open device connection
    /// </summary>
    void Open();

    /// <summary>
    /// Close connection
    /// </summary>
    void Close();

    /// <summary>
    /// Execute command
    /// </summary>
    /// <param name="command">Command to execute</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Exec(ICommand command, CancellationToken cancellationToken = new());

    /// <summary>
    /// Write data with <see cref="IDataWriter{T}"/>
    /// </summary>
    /// <param name="writer"><see cref="IDataWriter{T}"/></param>
    /// <param name="data">Data to write</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <typeparam name="TData">Type of data</typeparam>
    /// <returns></returns>
    Task Write<TData>(IDataWriter<TData> writer, TData data, CancellationToken cancellationToken = new());
    
    /// <summary>
    /// Read data with <see cref="IDataReader{T}"/>
    /// </summary>
    /// <param name="reader"><see cref="IDataReader{T}"/></param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <typeparam name="TData">Type of data</typeparam>
    /// <returns></returns>
    Task<TData?> Read<TData>(IDataReader<TData> reader, CancellationToken cancellationToken = new());
}