﻿using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract; 

public interface IDeviceWatcher {
    
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