using System;
using System.IO;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Transport.Interfaces;

/// <summary>
/// Client which make connection to device
/// </summary>
/// <typeparam name="TConnectionParams">Type of device connection parameters</typeparam>
public interface IClient<in TConnectionParams> : IDisposable {
    bool Connected { get; }
    void Close();

    Stream GetStream();

    void Connect(TConnectionParams connectionParams);
    
    Task ConnectAsync(TConnectionParams connectionParams);
}