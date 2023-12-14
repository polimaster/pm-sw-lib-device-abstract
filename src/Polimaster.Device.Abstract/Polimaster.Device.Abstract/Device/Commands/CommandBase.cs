using System;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport.Stream;

namespace Polimaster.Device.Abstract.Device.Commands; 

/// <summary>
/// 
/// </summary>
public abstract class CommandBase<T> {
    /// <summary>
    /// Command logger
    /// </summary>
    protected ILogger? Logger { get; }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="loggerFactory"></param>
    protected CommandBase(ILoggerFactory? loggerFactory) {
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
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="stream"></param>
    /// <typeparam name="TStream"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    protected IDeviceStream<T> GetStream<TStream>(TStream stream) {
        if (stream is not IDeviceStream<T> str)
            throw new ArgumentException($"Parameter {nameof(stream)} should implement {typeof(IDeviceStream<T>)}, actual type is {typeof(TStream)}");
        return str;
    }
}