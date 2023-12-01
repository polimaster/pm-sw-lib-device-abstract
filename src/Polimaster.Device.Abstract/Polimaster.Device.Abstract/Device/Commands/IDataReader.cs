using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <summary>
/// Device data reader
/// </summary>
/// <typeparam name="T">Type of data to read</typeparam>
public interface IDataReader<T> {
    /// <summary>
    /// Read data
    /// </summary>
    /// <param name="stream">Transport <see cref="IDeviceStream{T}"/></param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <typeparam name="TStream">Stream type</typeparam>
    /// <returns>Data from stream</returns>
    Task<T?> Read<TStream>(TStream stream, CancellationToken cancellationToken);
}