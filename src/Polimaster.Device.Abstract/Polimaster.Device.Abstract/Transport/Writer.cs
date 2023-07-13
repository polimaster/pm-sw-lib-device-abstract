using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Transport;

public class Writer : StreamWriter, IWriter {
    public Writer(Stream stream) : base(stream) {
    }

    public async Task WriteLineAsync(char[] chars, CancellationToken cancellationToken) {
        await base.WriteLineAsync(chars, cancellationToken);
    }
}