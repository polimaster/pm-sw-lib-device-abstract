using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Transport;

/// <summary>
/// Client which make connection to device
/// </summary>
/// <typeparam name="TConnectionParams">Type of device connection parameters</typeparam>
public interface IClient<in TConnectionParams> : IDisposable {
    bool Connected { get; }
    ILoggerFactory? LoggerFactory { get; set; }
    void Close();

    Task<IDeviceStream> GetStream();

    void Open(TConnectionParams connectionParams);
    
    Task OpenAsync(TConnectionParams connectionParams);
    
    Action? Opened { get; set; }
    Action? Closed { get; set; }
}