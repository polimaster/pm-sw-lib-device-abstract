using System;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Transport;

/// <summary>
/// Device transport layer (USB, Tcp, Bluetooth etc)
/// </summary>
public interface ITransport : IDisposable {
    /// <summary>
    /// Connection identifier
    /// </summary>
    string ConnectionId { get; }

    /// <summary>
    /// Open device stream reader/writer
    /// </summary>
    /// <returns></returns>
    Task<IDeviceStream> Open();

    /// <summary>
    /// Close connection
    /// </summary>
    Task Close();

    /// <summary>
    /// Occurs when connection opened
    /// </summary>
    Action? Opened { get; set; }

    /// <summary>
    /// Occurs when connection closed
    /// </summary>
    Action? Closed { get; set; }
}

/// <inheritdoc cref="ITransport"/>
/// <typeparam name="TConnectionParams">Parameters while connecting to device</typeparam>
/// <typeparam name="TClient">Client type</typeparam>
public interface ITransport<out TClient, out TConnectionParams> : ITransport
    where TClient : IClient<TConnectionParams>, new() {
    /// <summary>
    /// Transport client
    /// </summary>
    TClient? Client { get; }

    /// <summary>
    /// Parameters for connection
    /// </summary>
    TConnectionParams? ConnectionParams { get; }
}