using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Transport.Stream.Socket;

/// <summary>
/// <see cref="NetworkStream"/> interface adapter
/// </summary>
public interface ISocketStream {
    /// <summary>
    /// See <see cref="NetworkStream.DataAvailable"/>
    /// </summary>
    bool DataAvailable { get; }

    /// <summary>
    /// See <see cref="Stream.WriteAsync(byte[],int,int,System.Threading.CancellationToken)"/>
    /// </summary>
    Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken);

    /// <summary>
    /// See <see cref="Stream.FlushAsync(System.Threading.CancellationToken)"/>
    /// </summary>
    Task FlushAsync(CancellationToken cancellationToken);

    /// <summary>
    /// See <see cref="Stream.ReadAsync(byte[],int,int,System.Threading.CancellationToken)"/>
    /// </summary>
    Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken);
}