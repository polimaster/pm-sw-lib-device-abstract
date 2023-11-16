using System;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device.Commands;

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
}