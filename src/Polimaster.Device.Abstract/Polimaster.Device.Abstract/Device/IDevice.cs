using System;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Commands;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device; 

/// <summary>
/// Device
/// </summary>
/// <typeparam name="TData" cref="ICommand{TParam,TData}"></typeparam>
public interface IDevice<TData> : IDisposable {

    /// <summary>
    /// Transport layer
    /// </summary>
    /// <see cref="ITransport{TData}"/>
    ITransport<TData> Transport { get; }

    /// <summary>
    /// Send command to device
    /// </summary>
    /// <param name="command">
    ///     <see cref="ICommand{TParam,TData}"/>
    ///     Command to be send to device</param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TParam"><see cref="ICommand{TParam,TData}"/></typeparam>
    Task Write<TParam>(ICommand<TParam, TData> command, CancellationToken cancellationToken = new());
    

    /// <summary>
    /// Reads data from device with command
    /// </summary>
    /// <param name="command">
    ///     <see cref="IReadCommand{TResult,TParam,TData}"/>
    ///     Command to be send to device
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TResult"><see cref="IReadCommand{TResult,TParam,TData}"/></typeparam>
    /// <typeparam name="TParam"><see cref="IReadCommand{TResult,TParam,TData}"/></typeparam>
    Task<TResult?> Read<TResult, TParam>(IReadCommand<TResult, TParam, TData> command, CancellationToken cancellationToken = new());
}