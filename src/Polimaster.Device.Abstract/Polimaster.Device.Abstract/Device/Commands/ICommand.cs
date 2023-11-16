using System;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <summary>
/// Device command
/// </summary>
public interface ICommand {
    /// <summary>
    /// Send command to device
    /// </summary>
    /// <param name="stream">Device stream</param>
    /// <param name="sleep">Amount of milliseconds to sleep after command execution</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns></returns>
    Task Send(IDeviceStream stream, ushort sleep = 0, CancellationToken cancellationToken = new());
}


/// <summary>
/// Device command
/// </summary>
/// <typeparam name="TValue">Type of <see cref="Value"/></typeparam>
public interface ICommand<TValue> : ICommand {
    /// <summary>
    /// Value of command. Either result of execution or it parameter.
    /// </summary>
    TValue? Value { get; set; }

    /// <summary>
    /// Occurs when value is changed
    /// </summary>
    Action<TValue?>? ValueChanged { get; set; }
}