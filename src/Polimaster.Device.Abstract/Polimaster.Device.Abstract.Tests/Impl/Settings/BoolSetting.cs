using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Tests.Impl.Commands;
using Polimaster.Device.Abstract.Tests.Impl.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Settings;

public class BoolSetting(
    IMyTransport transport,
    IMySettingDescriptors settingsDescriptors,
    ILoggerFactory? loggerFactory)
    : ADeviceSetting<bool>(new SettingDefinition<bool> {
        Reader = new BoolReader(transport, loggerFactory),
        Descriptor = settingsDescriptors.BoolSettingDescriptor,
        Writer = new BoolWriter(transport, loggerFactory)
    });