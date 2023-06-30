using System;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Commands;

namespace Polimaster.Device.Abstract.Device.Settings;

/// <summary>
/// Device setting
/// </summary>
/// <typeparam name="T">Type of setting value</typeparam>
/// <typeparam name="TData"></typeparam>
public interface IDeviceSetting<T, TData> {
    
    ICommand<T, TData>? ReadCommand { get; set; }
    ICommand<T, TData>? WriteCommand { get; set; }

    /// <summary>
    /// Setting value
    /// </summary>
    T? Value { get; }
    
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
    Task CommitChanges(CancellationToken cancellationToken);
}