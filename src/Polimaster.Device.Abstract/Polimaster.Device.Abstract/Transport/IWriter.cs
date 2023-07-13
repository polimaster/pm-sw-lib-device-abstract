using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Transport;

public interface IWriter {
    Task WriteLineAsync(char[] chars, CancellationToken cancellationToken);
    Stream BaseStream { get; }
}