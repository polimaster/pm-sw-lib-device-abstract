using System.IO;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Transport;

public interface IReader {
    Stream BaseStream { get; }
    Task<string> ReadToEndAsync();

    Task<string?> ReadLineAsync();
}