using System;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Transport;

/// <summary>
/// Client which make connection to device
/// </summary>
/// <typeparam name="TConnectionParams">Type of device connection parameters</typeparam>
public interface IClient<in TConnectionParams> : IDisposable {
    bool Connected { get; }
    void Close();

    Task<IDeviceStream> GetStream();

    void Open(TConnectionParams connectionParams);
    
    Task OpenAsync(TConnectionParams connectionParams);
}