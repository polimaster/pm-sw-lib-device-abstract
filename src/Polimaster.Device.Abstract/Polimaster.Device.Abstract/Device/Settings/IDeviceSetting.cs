using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Settings;

/// <summary>
/// Device setting
/// </summary>
/// <typeparam name="T">Type of setting <see cref="Value"/></typeparam>
public interface IDeviceSetting<T> {

    /// <summary>
    /// See <see cref="ISettingBehaviour"/>
    /// </summary>
    ISettingBehaviour? Behaviour { get; }
    
    /// <summary>
    /// Indicates if setting is readonly
    /// </summary>
    bool ReadOnly { get; }

    /// <summary>
    /// Setting value
    /// </summary>
    T? Value { get; set; }
    
    /// <summary>
    /// Indicates if setting changed via setting <see cref="Value"/> and ready to <see cref="CommitChanges"/>
    /// </summary>
    bool IsDirty { get; }
    
    /// <summary>
    /// Indicates if setting read from device.
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
    IEnumerable<ValidationResult>? ValidationErrors { get; }

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
    /// Force to read setting from device
    /// </summary>
    /// <returns></returns>
    Task Reset(CancellationToken cancellationToken = new());

    /// <summary>
    /// Writes <see cref="Value"/> to device if it <see cref="IsDirty"/>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task CommitChanges(CancellationToken cancellationToken = new());
}