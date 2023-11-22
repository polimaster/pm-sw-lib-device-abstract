using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Tests.Impl.Device; 

public class MyDeviceManager : ADeviceManager<MyDevice> {
    public MyDeviceManager(ILoggerFactory? loggerFactory) : base(loggerFactory) {
    }
}