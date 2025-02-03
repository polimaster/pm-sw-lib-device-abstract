using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <summary>
/// Device data writer
/// </summary>
/// <typeparam name="T">Type of data to write</typeparam>
public interface IDataWriter<in T> {
    /// <summary>
    /// Write data
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <param name="data">Data to write</param>
    /// <returns></returns>
    Task Write(T data, CancellationToken cancellationToken);
}