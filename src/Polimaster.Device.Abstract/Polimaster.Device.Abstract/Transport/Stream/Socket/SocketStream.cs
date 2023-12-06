using System.Net.Sockets;

namespace Polimaster.Device.Abstract.Transport.Stream.Socket;

/// <inheritdoc cref="ISocketStream" />
public class SocketStream : NetworkStream, ISocketStream {
    /// <inheritdoc />
    public SocketStream(System.Net.Sockets.Socket socket) : base(socket) {
    }

    /// <inheritdoc />
    public SocketStream(System.Net.Sockets.Socket socket, bool ownsSocket) : base(socket, ownsSocket) {
    }
}