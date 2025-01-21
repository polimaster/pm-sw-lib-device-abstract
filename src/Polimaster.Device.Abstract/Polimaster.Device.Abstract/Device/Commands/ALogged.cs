using System;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <summary>
/// Base command class with logging capabilities
/// </summary>
public abstract class ALogged {
    /// <summary>
    ///
    /// </summary>
    /// <param name="loggerFactory"></param>
    protected ALogged(ILoggerFactory? loggerFactory) {
        Logger = loggerFactory?.CreateLogger(GetType());
    }

    /// <summary>
    /// Command logger
    /// </summary>
    protected ILogger? Logger { get; }

    /// <summary>
    /// Log current command
    /// </summary>
    /// <param name="methodName"></param>
    protected void LogDebug(string methodName) => Logger?.LogDebug("Call {N} with command {C}", methodName, GetType().Name);

    /// <summary>
    /// Log error
    /// </summary>
    /// <param name="e"></param>
    /// <param name="methodName"></param>
    protected void LogError(Exception e, string methodName) => Logger?.LogError(e, "Error while sending {N} command {C}",methodName, GetType().Name);
}