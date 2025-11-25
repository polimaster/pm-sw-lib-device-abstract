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
    /// <param name="commandName"></param>
    protected void LogDebug(string commandName) => Logger?.LogDebug("Execute {C}", commandName);

    /// <summary>
    /// Log error
    /// </summary>
    /// <param name="e"></param>
    /// <param name="commandName"></param>
    protected void LogError(Exception e, string commandName) => Logger?.LogError(e, "Error while executing {C}", commandName);
}