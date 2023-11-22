using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Transport; 

public class MyClient : AClient<string, MemoryStreamParams> {
    private readonly MemoryStreamParams _memoryStreamParams;
    private MemoryStream? _memory;

    public MyClient(MemoryStreamParams memoryStreamParams, ILoggerFactory? loggerFactory) : base(memoryStreamParams, loggerFactory) {
        _memoryStreamParams = memoryStreamParams;
    }

    public override void Dispose() {
        _memory?.Dispose();
        GC.SuppressFinalize(this);
    }

    public override bool Connected => _memory is { CanRead: true };

    public override void Close() {
        _memory?.Close();
    }

    public override IDeviceStream<string> GetStream() {
        if (_memory != null) return new MyDeviceStream(_memory, LoggerFactory);
        throw new NullReferenceException("MemoryStream is null");
    }

    public override void Open() {
        _memory = new MemoryStream(_memoryStreamParams.Capacity);
    }

    public override Task OpenAsync() {
        _memory = new MemoryStream();
        return Task.CompletedTask;
    }
}