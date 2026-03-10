using Polimaster.Device.Abstract.Device.Commands;

namespace Polimaster.Device.Abstract.Device.Settings;

/// <summary>
/// Setting definition
/// </summary>
/// <typeparam name="T">Data type for <see cref="IDataReader{T}"/> and <see cref="IDataWriter{T}"/></typeparam>
public class SettingDefinition<T> {
    /// <summary>
    /// Command for read data
    /// </summary>
    public required IDataReader<T> Reader { get; init; }

    /// <summary>
    /// Command for writing data. If null, it creates a readonly setting.
    /// </summary>
    public IDataWriter<T>? Writer { get; init; }

    /// <summary>
    /// See <see cref="ISettingDescriptor"/>
    /// </summary>
    public required ISettingDescriptor Descriptor { get; init; }
}