using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Transport.Stream;


/// <summary>
/// Stream for reading/writing data to device
/// </summary>
public interface IDeviceStream<T> {
    /// <summary>
    /// Write buffer to device stream
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns></returns>
    Task WriteAsync(T buffer, CancellationToken cancellationToken);
    
    /// <summary>
    /// Read from device stream
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns></returns>
    public Task<T> ReadAsync(CancellationToken cancellationToken);
}