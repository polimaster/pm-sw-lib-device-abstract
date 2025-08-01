using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Settings;

/// <summary>
/// Device setting
/// </summary>
/// <typeparam name="T">Data type of <see cref="Value"/></typeparam>
public interface IDeviceSetting<T> where T : notnull {

    /// <summary>
    /// See <see cref="ISettingBehaviour"/>
    /// </summary>
    ISettingBehaviour? Behaviour { get; }
    
    /// <summary>
    /// Indicates if the setting is readonly
    /// </summary>
    bool ReadOnly { get; }

    /// <summary>
    /// Setting value
    /// </summary>
    T? Value { get; set; }

    /// <summary>
    /// Gets a value indicating whether the current <see cref="Value"/> is set via
    /// assigning to <see cref="Value"/> itself either with <see cref="Read"/> or <see cref="Reset"/> metods.
    /// </summary>
    bool HasValue { get; }
    
    /// <summary>
    /// Indicates if setting changed via setting <see cref="Value"/> and ready to <see cref="CommitChanges"/>
    /// </summary>
    bool IsDirty { get; }
    
    /// <summary>
    /// Indicates if setting read from the device.
    /// </summary>
    bool IsSynchronized { get; }
    
    /// <summary>
    /// Indicates if <see cref="Value"/> valid
    /// </summary>
    bool IsValid { get; }
    
    /// <summary>
    /// Check if <see cref="Exception"/> is not null while <see cref="Read"/> or <see cref="CommitChanges"/> operations
    /// </summary>
    bool IsError { get; }
    
    /// <summary>
    /// <see cref="Value"/> validation errors
    /// </summary>
    List<ValidationResult> ValidationErrors { get; }

    /// <summary>
    /// Error occured while performing <see cref="Read"/> or <see cref="CommitChanges"/> operations
    /// </summary>
    Exception? Exception { get; }

    /// <summary>
    /// Reads setting from the device
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns></returns>
    Task Read(CancellationToken cancellationToken);

    /// <summary>
    /// Force to read setting from the device
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns></returns>
    Task Reset(CancellationToken cancellationToken);

    /// <summary>
    /// Writes <see cref="Value"/> to the device if it <see cref="IsDirty"/>
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns></returns>
    Task CommitChanges(CancellationToken cancellationToken);
}