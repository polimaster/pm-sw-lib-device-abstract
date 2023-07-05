using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Interfaces;
using Polimaster.Device.Abstract.Transport.Interfaces;

namespace Polimaster.Device.Abstract.Device.Commands.Interfaces;

/// <summary>
/// Device command
/// </summary>
/// <typeparam name="T">Type of <see cref="Value"/></typeparam>
public interface ICommand<T> {
    /// <summary>
    /// Send command to device
    /// </summary>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns></returns>
    Task Send(CancellationToken cancellationToken = new ());
    
    /// <summary>
    /// Value of command. Either result of execution or it parameter.
    /// </summary>
    T? Value { get; set; }
    
    Action<T?>? ValueChanged { get; set; }
    
    /// <summary>
    /// Logger
    /// </summary>
    ILogger? Logger { get; set; }
}

/// <summary>
/// Device command
/// </summary>
/// <typeparam name="TTransport">Data type for <see cref="ITransport{T}"/></typeparam>
/// <typeparam name="T"><inheritdoc cref="ICommand{TValue}"/></typeparam>
public interface ICommand<T, TTransport> : ICommand<T> {

    /// <summary>
    /// Device command belongs to
    /// </summary>
    IDevice<TTransport> Device { get; set; }
    
    /// <summary>
    /// Underlying command builder in case if command should have underlying commands to execute.
    /// </summary>
    ICommandBuilder<TTransport> CommandBuilder { get; set; }
}