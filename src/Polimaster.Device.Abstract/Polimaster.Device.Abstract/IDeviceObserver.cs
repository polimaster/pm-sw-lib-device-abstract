using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract; 


/// <summary>
/// Device observer finds devices on target (USB, IrDA, Bluetooth, etc)
/// </summary>
public interface IDeviceObserver {
    
    /// <summary>
    /// Starts finding devices in background
    /// </summary>
    /// <param name="token">Cancellation token</param>
    /// <param name="timeout">Cycle timeout</param>
    Task WatchForDevicesStart(CancellationToken token, int timeout = 20);

    /// <summary>
    /// Stops finding devices in background
    /// </summary>
    Task WatchForDevicesStop();
}