using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Transport; 

public interface IDeviceStream {
    Stream BaseStream { get; }
    
    Task WriteLineAsync(string value, CancellationToken cancellationToken);
    Task<string> ReadLineAsync(CancellationToken cancellationToken);

    public Task WriteAsync(byte[] buffer, CancellationToken cancellationToken);
    public Task<byte[]> ReadAsync(CancellationToken cancellationToken);
}