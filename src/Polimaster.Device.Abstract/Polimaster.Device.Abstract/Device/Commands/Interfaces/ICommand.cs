using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport.Interfaces;

namespace Polimaster.Device.Abstract.Device.Commands.Interfaces;

/// <summary>
/// Device command
/// </summary>
/// <typeparam name="TValue">Type of <see cref="Value"/></typeparam>
public interface ICommand<TValue> {
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
    
    Action<TValue?>? ValueChanged { get; set; }
    
    /// <summary>
    /// Logger
    /// </summary>
    ILogger? Logger { get; set; }
}

/// <summary>
/// Device command
/// </summary>
/// <typeparam name="TTransportData"><see cref="ITransport{TData}"/></typeparam>
/// <typeparam name="TValue"><inheritdoc cref="ICommand{TValue}"/></typeparam>
public interface ICommand<TValue, TTransportData> : ICommand<TValue> {

    /// <summary>
    /// Command transport
    /// </summary>
    ITransport<TTransportData>? Transport { get; set; }
}