using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Transport.Stream;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <summary>
/// Device data writer
/// </summary>
/// <typeparam name="T">Type of data to write</typeparam>
public interface IDataWriter<in T> {
    /// <summary>
    /// Write data
    /// </summary>
    /// <param name="stream">Transport <see cref="IDeviceStream{T}"/></param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <typeparam name="TStream">Stream type</typeparam>
    /// <param name="data">Data to write</param>
    /// <returns></returns>
    Task Write<TStream>(TStream stream, T data, CancellationToken cancellationToken);
}