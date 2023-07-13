using System;
using System.IO;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Transport;

/// <summary>
/// Device transport layer (USB, Tcp, Bluetooth etc)
/// </summary>
public interface ITransport : IDisposable {
    
    string ConnectionId { get; }

    /// <summary>
    /// Open connection
    /// </summary>
    Stream Open();

    /// <summary>
    /// Connection stream writer
    /// </summary>
    /// <returns><see cref="StreamWriter"/></returns>
    IWriter GetWriter();
    
    /// <summary>
    /// Connection stream reader
    /// </summary>
    /// <returns><see cref="StreamReader"/></returns>
    IReader GetReader();

    /// <summary>
    /// Close connection
    /// </summary>
    Task Close();
}

/// <inheritdoc cref="ITransport"/>
/// <typeparam name="TConnectionParams">Parameters while connecting to device</typeparam>
public interface ITransport<TConnectionParams> : ITransport {
    
    IClient<TConnectionParams> Client { get; }

    /// <summary>
    /// Parameters for connection
    /// </summary>
    TConnectionParams? ConnectionParams { get; }
    
}