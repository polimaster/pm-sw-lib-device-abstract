using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Transport.Http; 

public class TcpStream : IDeviceStream{
    private readonly NetworkStream _networkStream;

    public TcpStream(NetworkStream networkStream) {
        _networkStream = networkStream;
    }

    public Stream BaseStream { get; }
    public async Task WriteLineAsync(string value, CancellationToken cancellationToken) {
        throw new System.NotImplementedException();
    }

    public async Task<string> ReadLineAsync(CancellationToken cancellationToken) {
        throw new System.NotImplementedException();
    }

    public string ReadLine() {
        throw new System.NotImplementedException();
    }

    public async Task WriteAsync(byte[] buffer, CancellationToken cancellationToken) {
        throw new System.NotImplementedException();
    }

    public async Task<byte[]> ReadAsync(CancellationToken cancellationToken) {
        throw new System.NotImplementedException();
    }
}