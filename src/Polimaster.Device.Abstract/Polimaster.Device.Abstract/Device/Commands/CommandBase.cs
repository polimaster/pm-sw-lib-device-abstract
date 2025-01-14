using System;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Commands; 

/// <summary>
/// 
/// </summary>
public abstract class CommandBase {

    /// <summary>
    /// <see cref="ITransport"/> layer for executing command
    /// </summary>
    protected ITransport Transport { get; }


    /// <summary>
    /// Command logger
    /// </summary>
    protected ILogger? Logger { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="transport"></param>
    /// <param name="loggerFactory"></param>
    protected CommandBase(ITransport transport, ILoggerFactory? loggerFactory) {
        Transport = transport;
        Logger = loggerFactory?.CreateLogger(GetType());
    }
    
    /// <summary>
    /// Log current command
    /// </summary>
    /// <param name="methodName"></param>
    protected void LogCommand(string methodName) => Logger?.LogDebug("Call {N} with command {C}", methodName, GetType().Name);

    /// <summary>
    /// Log error
    /// </summary>
    /// <param name="e"></param>
    /// <param name="methodName"></param>
    protected void LogError(Exception e, string methodName) => Logger?.LogError(e, "Error while sending {N} command {C}",methodName, GetType().Name);
}