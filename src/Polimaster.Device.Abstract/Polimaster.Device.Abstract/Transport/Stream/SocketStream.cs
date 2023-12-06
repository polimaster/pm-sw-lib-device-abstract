using System.Net.Sockets;

namespace Polimaster.Device.Abstract.Transport.Stream;

/// <inheritdoc cref="ISocketStream" />
public class SocketStream : NetworkStream, ISocketStream {
    /// <inheritdoc />
    public SocketStream(Socket socket) : base(socket) {
    }

    /// <inheritdoc />
    public SocketStream(Socket socket, bool ownsSocket) : base(socket, ownsSocket) {
    }
}