using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <summary>
/// Command base class with <see cref="ITransport{TStream}"/>
/// </summary>
public abstract class CommandBase<TStream> : ALogged {

    /// <summary>
    /// <see cref="ITransport{TStream}"/> layer for executing command
    /// </summary>
    protected ITransport<TStream> Transport { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="transport"></param>
    /// <param name="loggerFactory"></param>
    protected CommandBase(ITransport<TStream> transport, ILoggerFactory? loggerFactory) : base(loggerFactory){
        Transport = transport;
    }
}