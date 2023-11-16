using System;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Transport; 

public class MyClient : AClient<ConnectionParams> {
    public override void Dispose() {
        throw new NotImplementedException();
    }

    public override bool Connected { get; }
    public override void Close() {
        throw new NotImplementedException();
    }

    public override async Task<IDeviceStream> GetStream() {
        throw new NotImplementedException();
    }

    public override void Open(ConnectionParams connectionParams) {
        throw new NotImplementedException();
    }

    public override async Task OpenAsync(ConnectionParams connectionParams) {
        throw new NotImplementedException();
    }

    public override Action? Opened { get; set; }
    public override Action? Closed { get; set; }
}