using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Interfaces;

namespace Polimaster.Device.Abstract.Device.Commands.Interfaces;

/// <summary>
/// Device command builder.
/// </summary>
public interface ICommandBuilder {

    /// <summary>
    /// Add logger to command
    /// </summary>
    /// <param name="logger">Logger</param>
    ICommandBuilder With(ILogger? logger);
    
    /// <summary>
    /// Add device to command
    /// </summary>
    /// <param name="device"><see cref="IDevice"/></param>
    /// <returns></returns>
    ICommandBuilder With(IDevice? device);

    /// <summary>
    /// Build command with identified value
    /// </summary>
    /// <param name="initialValue">Initial value for command</param>
    /// <typeparam name="T">Command implementation</typeparam>
    /// <typeparam name="TCommand">Type of <see cref="ICommand{T}"/></typeparam>
    /// <returns><see cref="T"/></returns>
    T Build<T, TCommand>(TCommand? initialValue = default) where T : class, ICommand<TCommand>, new();
    
    /// <summary>
    /// Build typeless command
    /// </summary>
    /// <typeparam name="T">Command implementation</typeparam>
    /// <returns><see cref="T"/></returns>
    T Build<T>() where T : class, ICommand, new();
}