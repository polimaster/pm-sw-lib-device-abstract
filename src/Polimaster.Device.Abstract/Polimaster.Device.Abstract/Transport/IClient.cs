using System;
using System.IO;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Transport;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TConnectionParams"></typeparam>
public interface IClient<in TConnectionParams> : IDisposable {
    bool Connected { get; }
    void Close();

    Stream GetStream();

    void Connect(TConnectionParams connectionParams);
    
    Task ConnectAsync(TConnectionParams connectionParams);
}