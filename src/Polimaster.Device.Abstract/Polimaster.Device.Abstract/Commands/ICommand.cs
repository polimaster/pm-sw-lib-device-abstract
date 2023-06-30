using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Commands;

/// <summary>
/// Device command
/// </summary>
/// <typeparam name="TValue">Type of <see cref="Value"/></typeparam>
/// <typeparam name="TTransportData"><see cref="ITransport{TData}"/></typeparam>
public interface ICommand<TValue, TTransportData> {
    
    /// <summary>
    /// Send command to device
    /// </summary>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns></returns>
    Task Send(CancellationToken cancellationToken = new ());
    
    /// <summary>
    /// Value of command. Either result of execution or it parameter.
    /// </summary>
    TValue? Value { get; set; }
    
    /// <summary>
    /// Command transport
    /// </summary>
    ITransport<TTransportData>? Transport { get; set; }
    
    /// <summary>
    /// Logger
    /// </summary>
    ILogger? Logger { get; set; }
}