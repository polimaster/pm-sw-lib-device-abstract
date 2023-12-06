using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using InTheHand.Net.Sockets;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Transport.Win.IrDA;

/// <summary>
/// IrDA stream implementation
/// </summary>
public class IrDAStream : IDeviceStream<byte[]> {
    private readonly ILogger<IrDAStream>? _logger;
    private readonly NetworkStream _stream;
    
    /// <summary>
    /// Buffer length while reading stream
    /// </summary>
    protected virtual int BuffLength => 256;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="client"></param>
    /// <param name="loggerFactory"></param>
    public IrDAStream(IrDAClient client, ILoggerFactory? loggerFactory = null) {
        _stream = client.GetStream();
        _logger = loggerFactory?.CreateLogger<IrDAStream>();
    }

    /// <inheritdoc />
    public virtual async Task WriteAsync(byte[] buffer, CancellationToken cancellationToken) {
        _logger?.LogDebug("Call {F} with: {V} bytes", nameof(WriteAsync), buffer.Length);
        await _stream.WriteAsync(buffer, 0, buffer.Length, cancellationToken);
        await _stream.FlushAsync(cancellationToken);
    }

    /// <inheritdoc />
    public virtual async Task<byte[]> ReadAsync(CancellationToken cancellationToken) {
        _logger?.LogDebug("Call: {F}", nameof(ReadAsync));
        
        // var buff = new byte[10000];
        // var len = await _stream.ReadAsync(buff, 0, BUFF_LENGTH, cancellationToken);
        //
        // var data = new byte[len];
        // for (var i = 0; i < len; i++) data[i] = buff[i];
        // return data;

        var result = new List<byte>();
        
        var buffer = new byte[BuffLength];
        do {
            var bytes = await _stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
            if (bytes == 0) continue;

            result.AddRange(buffer);
        } while (_stream.DataAvailable);

        return result.ToArray();
    }
}