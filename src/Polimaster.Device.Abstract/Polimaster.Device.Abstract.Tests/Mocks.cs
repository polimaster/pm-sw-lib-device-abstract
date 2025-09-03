using System.Threading;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Tests.Impl.Settings;

namespace Polimaster.Device.Abstract.Tests; 

public class Mocks {
    protected static readonly ILoggerFactory? LOGGER_FACTORY = LoggerFactory.Create(builder => builder.AddConsole());
    protected readonly CancellationToken Token = CancellationToken.None;
    protected static readonly IMySettingDescriptors SETTING_DESCRIPTORS = new MySettingDescriptors();
}