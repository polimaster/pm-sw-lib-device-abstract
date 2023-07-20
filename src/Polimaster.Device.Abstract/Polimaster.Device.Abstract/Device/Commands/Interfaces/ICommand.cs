using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Interfaces;

namespace Polimaster.Device.Abstract.Device.Commands.Interfaces;


public interface ICommand {
    /// <summary>
    /// Send command to device
    /// </summary>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns></returns>
    Task Send(CancellationToken cancellationToken = new ());
    
    /// <summary>
    /// Logger
    /// </summary>
    ILogger? Logger { get; set; }
    
    /// <summary>
    /// Device command belongs to
    /// </summary>
    IDevice Device { get; set; }
}


/// <summary>
/// Device command
/// </summary>
/// <typeparam name="T">Type of <see cref="Value"/></typeparam>
public interface ICommand<T> : ICommand {
    /// <summary>
    /// Value of command. Either result of execution or it parameter.
    /// </summary>
    T? Value { get; set; }

    Action<T?>? ValueChanged { get; set; }
}