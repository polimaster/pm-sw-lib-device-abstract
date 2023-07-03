using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Interfaces;
using Polimaster.Device.Abstract.Transport.Interfaces;

namespace Polimaster.Device.Abstract.Device.Commands.Interfaces;


/// <summary>
/// Device command builder. Make sure you are creating singleton for each
/// <see cref="ICommandBuilder{TTransport}"/> implementation in order to cache commands.
/// </summary>
/// <typeparam name="TTransport">Type of <see cref="ITransport{T}"/></typeparam>
public interface ICommandBuilder<TTransport> {

    /// <summary>
    /// Add logger to command
    /// </summary>
    /// <param name="logger">Logger</param>
    /// <returns><see cref="ICommandBuilder{TTransport}"/></returns>
    ICommandBuilder<TTransport> With(ILogger? logger);

    /// <summary>
    /// Build command
    /// </summary>
    /// <param name="device">Target <see cref="IDevice{T}"/></param>
    /// <typeparam name="T">Command implementation</typeparam>
    /// <typeparam name="TCommand">Type of <see cref="ICommand{T}"/></typeparam>
    /// <returns></returns>
    T Build<T, TCommand>(IDevice<TTransport> device)
        where T : class, ICommand<TCommand, TTransport>, new();
}