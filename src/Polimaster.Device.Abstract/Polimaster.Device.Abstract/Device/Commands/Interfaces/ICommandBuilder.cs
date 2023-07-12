using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Interfaces;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Commands.Interfaces;

/// <summary>
/// Creates <see cref="ICommandBuilder{TTransport}"/> for target device
/// </summary>
public interface ICommandBuilder {
    
    /// <summary>
    /// Creates <see cref="ICommandBuilder{TTransport}"/> for target device
    /// </summary>
    /// <param name="device">Target <see cref="IDevice{T}"/></param>
    /// <typeparam name="TTransport">Device <see cref="ITransport{T}"/> type</typeparam>
    /// <returns><see cref="ICommandBuilder{TTransport}"/></returns>
    ICommandBuilder<TTransport> Create<TTransport>(IDevice<TTransport> device);
}


/// <summary>
/// Device command builder.
/// </summary>
/// <typeparam name="TTransport">Type of <see cref="ITransport{T}"/></typeparam>
public interface ICommandBuilder<TTransport> {

    /// <summary>
    /// Target device
    /// </summary>
    IDevice<TTransport>? Device { get; set; }
    
    /// <summary>
    /// Add logger to command
    /// </summary>
    /// <param name="logger">Logger</param>
    /// <returns><see cref="ICommandBuilder{TTransport}"/></returns>
    ICommandBuilder<TTransport> With(ILogger? logger);

    /// <summary>
    /// Build command
    /// </summary>
    /// <param name="initialValue">Initial value for command</param>
    /// <typeparam name="T">Command implementation</typeparam>
    /// <typeparam name="TCommand">Type of <see cref="ICommand{T}"/></typeparam>
    /// <returns></returns>
    T Build<T, TCommand>(TCommand? initialValue = default) where T : class, ICommand<TCommand, TTransport>, new();
}