using System;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device.Commands.Interfaces;

namespace Polimaster.Device.Abstract.Device.Settings.Interfaces;

/// <summary>
/// Device setting
/// </summary>
/// <typeparam name="T">Type of setting <see cref="Value"/></typeparam>
public interface IDeviceSetting<T> {
    
    /// <summary>
    /// Command for read data
    /// </summary>
    ICommand<T>? ReadCommand { get; set; }
    
    /// <summary>
    /// Command for write data
    /// </summary>
    ICommand<T>? WriteCommand { get; set; }
    
    /// <summary>
    /// Setting value
    /// </summary>
    T? Value { get; set; }
    
    /// <summary>
    /// Indicates if <see cref="Value"/> changed
    /// </summary>
    bool IsDirty { get; }
    
    /// <summary>
    /// Check if <see cref="Exception"/> is not null while <see cref="Read"/> or <see cref="CommitChanges"/> operations
    /// </summary>
    bool IsError { get; }
    
    /// <summary>
    /// Error while <see cref="Read"/> or <see cref="CommitChanges"/> operations
    /// </summary>
    Exception? Exception { get; }

    /// <summary>
    /// Reads setting from device
    /// </summary>
    /// <returns></returns>
    Task Read(CancellationToken cancellationToken = new());

    /// <summary>
    /// Writes <see cref="Value"/> to device if it <see cref="IsDirty"/>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task CommitChanges(CancellationToken cancellationToken = new());
}