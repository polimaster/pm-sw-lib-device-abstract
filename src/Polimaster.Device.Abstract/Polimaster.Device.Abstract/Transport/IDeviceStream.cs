using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Transport; 

/// <summary>
/// Stream for reading/writing data to device
/// </summary>
public interface IDeviceStream {
    
    /// <summary>
    /// Underlying data stream
    /// </summary>
    Stream BaseStream { get; }
    
    /// <summary>
    /// Write line to device stream
    /// </summary>
    /// <param name="value"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task WriteLineAsync(string value, CancellationToken cancellationToken);
    
    /// <summary>
    /// Read line from device stream
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<string> ReadLineAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Write byte buffer to device stream
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task WriteAsync(byte[] buffer, CancellationToken cancellationToken);
    
    /// <summary>
    /// Read byte buffer from device stream
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<byte[]> ReadAsync(CancellationToken cancellationToken);
}