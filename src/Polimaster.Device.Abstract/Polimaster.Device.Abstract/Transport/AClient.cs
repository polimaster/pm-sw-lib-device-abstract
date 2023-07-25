using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Transport;

public abstract class AClient<TConnectionParams> : IClient<TConnectionParams> {
    public abstract void Dispose();
    public abstract bool Connected { get; }
    public ILoggerFactory? LoggerFactory { get; set; }
    public abstract void Close();
    public abstract Task<IDeviceStream> GetStream();
    public abstract void Open(TConnectionParams connectionParams);
    public abstract Task OpenAsync(TConnectionParams connectionParams);
    public abstract Action? Opened { get; set; }
    public abstract Action? Closed { get; set; }
}