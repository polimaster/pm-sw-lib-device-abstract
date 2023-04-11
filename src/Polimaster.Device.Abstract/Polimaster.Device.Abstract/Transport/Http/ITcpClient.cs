using System;
using System.IO;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Transport.Http;

public interface ITcpClient : IDisposable {
    bool Connected { get; }
    void Close();

    Stream GetStream();

    Task ConnectAsync(string ip, int port);
}