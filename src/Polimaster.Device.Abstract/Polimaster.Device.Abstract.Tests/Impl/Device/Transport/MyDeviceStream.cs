using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;
using Polimaster.Device.Abstract.Transport.Stream;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Transport;

public class MyDeviceStream : IDeviceStream<string> {
    private readonly MemoryStream _stream;
    private readonly ILogger<MyDeviceStream>? _logger;
    private const int MAX_DATA_LENGTH = 10000;

    public MyDeviceStream(MemoryStream stream, ILoggerFactory? loggerFactory) {
        _stream = stream;
        _logger = loggerFactory?.CreateLogger<MyDeviceStream>();
    }

    public async Task WriteAsync(string buffer, CancellationToken cancellationToken) {
        _logger?.LogDebug("Call {F} with: {V}", nameof(WriteAsync), buffer);
        var v = Encoding.UTF8.GetBytes(buffer);
        await _stream.WriteAsync(v.AsMemory(0, buffer.Length), cancellationToken);
        await _stream.FlushAsync(cancellationToken);
    }

    public async Task<string> ReadAsync(CancellationToken cancellationToken) {
        _logger?.LogDebug("Call: {F}", nameof(ReadAsync));
        var arr = await ReadBytesAsync(cancellationToken);
        return Encoding.UTF8.GetString(arr);
    }

    private async Task<byte[]> ReadBytesAsync(CancellationToken cancellationToken) {
        _logger?.LogDebug("Call: {F}", nameof(ReadAsync));
        
        var buff = new byte[MAX_DATA_LENGTH];
        var len = await _stream.ReadAsync(buff.AsMemory(0, MAX_DATA_LENGTH), cancellationToken);
    
        var data = new byte[len];
        for (var i = 0; i < len; i++) data[i] = buff[i];
        return data;
    }
}