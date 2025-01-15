using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <summary>
/// Command base class with <see cref="ITransport"/>
/// </summary>
public abstract class CommandBase : ALogged {

    /// <summary>
    /// <see cref="ITransport"/> layer for executing command
    /// </summary>
    protected ITransport Transport { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="transport"></param>
    /// <param name="loggerFactory"></param>
    protected CommandBase(ITransport transport, ILoggerFactory? loggerFactory) : base(loggerFactory){
        Transport = transport;
    }
}