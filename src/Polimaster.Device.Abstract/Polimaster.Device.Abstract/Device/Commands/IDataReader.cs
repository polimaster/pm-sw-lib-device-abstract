using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <summary>
/// Device data reader
/// </summary>
/// <typeparam name="T">Type of data to read</typeparam>
public interface IDataReader<T> where T: notnull {
    /// <summary>
    /// Read data
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>Data from stream</returns>
    Task<T> Read(CancellationToken cancellationToken);
}