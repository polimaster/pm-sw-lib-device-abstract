// using Polimaster.Device.Abstract.Transport;
//
// namespace Polimaster.Device.Abstract.Device.Interfaces; 
//
//
// /// <summary>
// /// Device builder. Make sure you are creating singleton for each
// /// <see cref="IDeviceBuilder"/> implementation in order to cache its data.
// /// </summary>
// public interface IDeviceBuilder {
//
//     /// <summary>
//     /// Add transport to device
//     /// </summary>
//     /// <param name="transport"><see cref="IDevice.Transport"/></param>
//     IDeviceBuilder With(ITransport transport);
//
//     /// <summary>
//     /// Build device
//     /// </summary>
//     /// <typeparam name="T">Device implementation</typeparam>
//     /// <returns><see cref="IDevice"/></returns>
//     T Build<T>() where T : class, IDevice, new();
// }