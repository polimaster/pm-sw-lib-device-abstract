using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Tests.Impl.Commands;
using Polimaster.Device.Abstract.Tests.Impl.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Settings;

public class OnOffSetting(
    IMyTransport transport,
    IMySettingDescriptors settingsDescriptors,
    ILoggerFactory? loggerFactory)
    : ADeviceSetting<OnOff>(new SettingDefinition<OnOff> {
        Reader = new OnOffReader(transport, loggerFactory),
        Descriptor = settingsDescriptors.OnOffSettingDescriptor,
        Writer = new OnOffWriter(transport, loggerFactory)
    });