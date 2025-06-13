using System;
using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Transport;

/// <summary>
/// Device transport layer (USB, Tcp, Bluetooth, etc.)
/// </summary>
/// <typeparam name="TStream">Stream type</typeparam>
public interface ITransport<TStream> : IDisposable {
    /// <summary>
    /// Connection identifier
    /// </summary>
    // string ConnectionId { get; }

    IClient<TStream> Client { get; }

    /// <summary>
    /// Indicates connection will be closed
    /// </summary>
    public event Action? Closing;

    /// <summary>
    /// Open device connection
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns></returns>
    Task Open(CancellationToken cancellationToken);

    /// <summary>
    /// Close connection
    /// </summary>
    void Close();

    /// <summary>
    /// Execute <paramref name="action"/> on <see cref="IClient{TStream}"/> stream
    /// </summary>
    /// <param name="action"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task ExecOnStream(Func<TStream, Task> action, CancellationToken cancellationToken);
}