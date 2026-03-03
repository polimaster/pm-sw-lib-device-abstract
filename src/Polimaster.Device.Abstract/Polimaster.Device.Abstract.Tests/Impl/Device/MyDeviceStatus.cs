using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Implementations.Status;
using Polimaster.Device.Abstract.Tests.Impl.Transport;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device;

public class MyStatus {
    public int Value { get; set; }
}

public class MyDeviceStatus : ADeviceStatus<MyStatus, IMyDeviceStream> {
    public bool IsStarted { get; private set; }
    public bool IsStopped { get; private set; }

    public MyDeviceStatus(ITransport<IMyDeviceStream> transport, ILoggerFactory? loggerFactory) : base(transport, loggerFactory) {
    }

    public override Task<MyStatus> Read(CancellationToken token) {
        return Task.FromResult(new MyStatus { Value = 42 });
    }

    public override void Start(CancellationToken token) {
        IsStarted = true;
    }

    public override void Stop() {
        IsStopped = true;
    }

    public override event Action<MyStatus>? HasNext;

    public void RaiseHasNext(MyStatus status) {
        HasNext?.Invoke(status);
    }
}
