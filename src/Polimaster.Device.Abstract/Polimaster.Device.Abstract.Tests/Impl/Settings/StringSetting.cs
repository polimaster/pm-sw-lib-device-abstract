using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Tests.Impl.Commands;
using Polimaster.Device.Abstract.Tests.Impl.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Settings; 

public class StringSetting(
    IMyTransport transport,
    IMySettingDescriptors settingDescriptor,
    ILoggerFactory? loggerFactory)
    : ADeviceSetting<string>(new SettingDefinition<string> {
        Reader = new PlainReader(transport, loggerFactory),
        Writer = new PlainWriter(transport, loggerFactory),
        Descriptor = settingDescriptor.StringSettingDescriptor
    });