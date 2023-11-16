using System;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Transport; 

public class MyClient : IClient<ConnectionParams> {
    public void Dispose() {
        throw new NotImplementedException();
    }

    public bool Connected { get; }
    public void Close() {
        throw new NotImplementedException();
    }

    public async Task<IDeviceStream> GetStream() {
        throw new NotImplementedException();
    }

    public void Open(ConnectionParams connectionParams) {
        throw new NotImplementedException();
    }

    public async Task OpenAsync(ConnectionParams connectionParams) {
        throw new NotImplementedException();
    }
}