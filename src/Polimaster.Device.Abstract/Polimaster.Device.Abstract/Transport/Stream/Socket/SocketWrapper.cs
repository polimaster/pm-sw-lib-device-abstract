using System.Net.Sockets;

namespace Polimaster.Device.Abstract.Transport.Stream.Socket;

/// <inheritdoc cref="ISocketStream" />
public class SocketWrapper : NetworkStream, ISocketStream {
    /// <inheritdoc />
    public SocketWrapper(System.Net.Sockets.Socket socket) : base(socket) {
    }

    /// <inheritdoc />
    public SocketWrapper(System.Net.Sockets.Socket socket, bool ownsSocket) : base(socket, ownsSocket) {
    }
}