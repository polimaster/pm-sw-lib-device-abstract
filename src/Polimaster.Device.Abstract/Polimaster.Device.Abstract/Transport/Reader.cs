using System.IO;

namespace Polimaster.Device.Abstract.Transport;

public class Reader : StreamReader, IReader {
    public Reader(Stream stream) : base(stream) {
    }
}