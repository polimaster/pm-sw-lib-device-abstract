// using Polimaster.Device.Abstract.Commands;
//
// namespace Polimaster.Device.Abstract.Device.Settings;
//
// /// <summary>
// /// Device settings factory
// /// </summary>
// /// <typeparam name="TData">Data type for <see cref="ICommand{TValue,TTransportData}.Transport"/></typeparam>
// public interface ISettingsFactory<TData> {  // todo add validation to setting
//     /// <summary>
//     /// Create device read-only setting
//     /// </summary>
//     /// <typeparam name="T">Type for <see cref="IDeviceSetting{T}"/></typeparam>
//     /// <typeparam name="TReadCommand">Read command <see cref="ICommand{TValue,TTransportData}"/></typeparam>
//     /// <returns></returns>
//     IDeviceSetting<T> Create<T, TReadCommand>() 
//         where TReadCommand : class, ICommand<T, TData>, new();
//
//
//     /// <summary>
//     /// Create device setting
//     /// </summary>
//     /// <typeparam name="T">Type for <see cref="IDeviceSetting{T}"/></typeparam>
//     /// <typeparam name="TReadCommand">Read command <see cref="ICommand{TValue,TTransportData}"/></typeparam>
//     /// <typeparam name="TWriteCommand">Write command <see cref="ICommand{TValue,TTransportData}"/></typeparam>
//     /// <returns></returns>
//     IDeviceSetting<T> Create<T, TReadCommand, TWriteCommand>() 
//         where TReadCommand : class, ICommand<T, TData>, new()
//         where TWriteCommand : class, ICommand<T, TData>, new();
//
//     /// <summary>
//     /// Create custom setting implementation 
//     /// </summary>
//     /// <typeparam name="T">Type for <see cref="IDeviceSetting{T}"/></typeparam>
//     /// <typeparam name="TImplementation">Implementation of <see cref="IDeviceSetting{T}"/></typeparam>
//     /// <typeparam name="TParams">Parameter type for <see cref="IDeviceSetting{T,TParams}.Init"/></typeparam>
//     /// <returns></returns>
//     IDeviceSetting<T> CreateCustom<T, TParams, TImplementation>(TParams @params)
//         where TImplementation : IDeviceSetting<T, TParams>, new();
// }
//
// public class SettingsFactory<TData> : ISettingsFactory<TData> {
//     private readonly ICommandFactory<TData> _commandFactory;
//
//     public SettingsFactory(ICommandFactory<TData> commandFactory) {
//         _commandFactory = commandFactory;
//     }
//
//     public IDeviceSetting<T> Create<T, TReadCommand>() 
//         where TReadCommand : class, ICommand<T, TData>, new() {
//         ICommand<T, TData> readCommand = _commandFactory.Create<TReadCommand, T>();
//         var setting = new DeviceSettingBase<T, TData>(readCommand);
//         return setting;
//     }
//
//     public IDeviceSetting<T> Create<T, TReadCommand, TWriteCommand>() 
//         where TReadCommand : class, ICommand<T, TData>, new()
//         where TWriteCommand : class, ICommand<T, TData>, new() {
//
//         ICommand<T, TData> readCommand = _commandFactory.Create<TReadCommand, T>();
//         ICommand<T,TData> writeCommand = _commandFactory.Create<TWriteCommand, T>();
//         
//         var setting = new DeviceSettingBase<T, TData>(readCommand, writeCommand);
//
//         return setting;
//     }
//
//     public IDeviceSetting<T> CreateCustom<T, TParams, TImplementation>(TParams @params) 
//         where TImplementation : IDeviceSetting<T, TParams>, new() {
//         var impl = new TImplementation();
//         impl.Init(@params);
//         return impl;
//     }
// }
