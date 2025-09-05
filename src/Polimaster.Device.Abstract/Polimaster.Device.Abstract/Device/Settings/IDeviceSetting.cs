using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Settings;


/// <summary>
/// Device setting
/// </summary>
public interface IDeviceSetting {

    /// <summary>
    /// Value type
    /// </summary>
    Type ValueType { get; }

    /// <summary>
    /// Untyped Value
    /// </summary>
    object? UntypedValue { get; set; }

    /// <summary>
    /// See <see cref="ISettingDescriptor"/>
    /// </summary>
    ISettingDescriptor Descriptor { get; }

    /// <summary>
    /// Indicates if the setting is readonly
    /// </summary>
    bool ReadOnly { get; }

    /// <summary>
    /// Gets a value indicating whether the current Value is set via
    /// assigning to Value itself either with <see cref="Read"/> or <see cref="Reset"/> metods.
    /// </summary>
    bool HasValue { get; }

    /// <summary>
    /// Indicates if setting changed via setting Value and ready to <see cref="CommitChanges"/>
    /// </summary>
    bool IsDirty { get; }

    /// <summary>
    /// Indicates if setting read from the device.
    /// </summary>
    bool IsSynchronized { get; }

    /// <summary>
    /// Indicates if Value valid
    /// </summary>
    bool IsValid { get; }

    /// <summary>
    /// Check if <see cref="Exception"/> is not null while <see cref="Read"/> or <see cref="CommitChanges"/> operations
    /// </summary>
    bool IsError { get; }

    /// <summary>
    /// Value validation errors
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
    /// Writes Value to the device if it <see cref="IsDirty"/>
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns></returns>
    Task CommitChanges(CancellationToken cancellationToken);
}


/// <summary>
/// Device setting
/// </summary>
/// <typeparam name="T">Data type of <see cref="Value"/></typeparam>
public interface IDeviceSetting<T> : IDeviceSetting where T : notnull {
    /// <summary>
    /// Setting value
    /// </summary>
    T? Value { get; set; }


}