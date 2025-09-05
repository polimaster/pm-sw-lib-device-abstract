using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Commands;


/// <summary>
/// Device command
/// </summary>
/// <typeparam name="TStream">See <see cref="ITransport{TStream}"/></typeparam>
public abstract class ACommand<TStream> : CommandBase<TStream>, ICommand {
    /// <inheritdoc />
    public virtual async Task Exec(CancellationToken cancellationToken) {
        // LogDebug(nameof(Exec));
        try {
            // await Transport.WriteAsync(Compile(), cancellationToken);
            await Transport.ExecOnStream(stream => Execute(stream, cancellationToken), cancellationToken);
        } catch (Exception e) {
            LogError(e, nameof(Exec));
            throw;
        }
    }

    /// <summary>
    /// Execute command on stream
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected abstract Task Execute(TStream stream, CancellationToken cancellationToken);

    /// <inheritdoc />
    protected ACommand(ITransport<TStream> transport, ILoggerFactory? loggerFactory) : base(transport, loggerFactory) {
    }
}