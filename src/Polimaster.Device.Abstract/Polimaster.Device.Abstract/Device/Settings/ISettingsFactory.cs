using Polimaster.Device.Abstract.Commands;

namespace Polimaster.Device.Abstract.Device.Settings;

/// <summary>
/// Device settings factory
/// </summary>
/// <typeparam name="TData">Data type for <see cref="ICommand{TValue,TTransportData}.Transport"/></typeparam>
public interface ISettingsFactory<TData> {
    /// <summary>
    /// Create device read-only setting
    /// </summary>
    /// <typeparam name="T">Type for <see cref="IDeviceSetting{T}"/></typeparam>
    /// <typeparam name="TReadCommand">Read command <see cref="ICommand{TValue,TTransportData}"/></typeparam>
    /// <returns></returns>
    IDeviceSetting<T> Create<T, TReadCommand>() 
        where TReadCommand : ICommand<T, TData>, new();


    /// <summary>
    /// Create device setting
    /// </summary>
    /// <typeparam name="T">Type for <see cref="IDeviceSetting{T}"/></typeparam>
    /// <typeparam name="TReadCommand">Read command <see cref="ICommand{TValue,TTransportData}"/></typeparam>
    /// <typeparam name="TWriteCommand">Write command <see cref="ICommand{TValue,TTransportData}"/></typeparam>
    /// <returns></returns>
    IDeviceSetting<T> Create<T, TReadCommand, TWriteCommand>() 
        where TReadCommand : ICommand<T, TData>, new()
        where TWriteCommand : ICommand<T, TData>, new();
    
    /// <summary>
    /// Create custom setting implementation 
    /// </summary>
    /// <typeparam name="T">Type for <see cref="IDeviceSetting{T}"/></typeparam>
    /// <typeparam name="TImplementation">Implementation of <see cref="IDeviceSetting{T}"/></typeparam>
    /// <returns></returns>
    IDeviceSetting<T> CreateCustomImpl<T, TImplementation>()
        where TImplementation : IDeviceSetting<T>, new();
}

public class SettingsFactory<TData> : ISettingsFactory<TData> {
    private readonly ICommandFactory<TData> _commandFactory;

    public SettingsFactory(ICommandFactory<TData> commandFactory) {
        _commandFactory = commandFactory;
    }

    public IDeviceSetting<T> Create<T, TReadCommand>() 
        where TReadCommand : ICommand<T, TData>, new() {
        ICommand<T, TData> readCommand = _commandFactory.Create<TReadCommand, T>();
        var setting = new DeviceSettingBase<T, TData>(readCommand);
        return setting;
    }

    public IDeviceSetting<T> Create<T, TReadCommand, TWriteCommand>() 
        where TReadCommand : ICommand<T, TData>, new()
        where TWriteCommand : ICommand<T, TData>, new() {

        ICommand<T, TData> readCommand = _commandFactory.Create<TReadCommand, T>();
        ICommand<T,TData> writeCommand = _commandFactory.Create<TWriteCommand, T>();
        
        var setting = new DeviceSettingBase<T, TData>(readCommand, writeCommand);

        return setting;
    }

    public IDeviceSetting<T> CreateCustomImpl<T, TImplementation>() 
        where TImplementation : IDeviceSetting<T>, new() {
        return new TImplementation();
    }
}