using System;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Tests.Impl.Commands;
using Polimaster.Device.Abstract.Tests.Impl.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Settings;

public class HistoryIntervalSetting(
    IMyTransport transport,
    IMySettingDescriptors settingsDescriptors,
    ILoggerFactory? loggerFactory)
    : ADeviceSetting<TimeSpan>(new SettingDefinition<TimeSpan> {
        Reader = new HistoryIntervalReader(transport, loggerFactory),
        Descriptor = settingsDescriptors.HistoryIntervalSettingDescriptor,
        Writer = new HistoryIntervalWriter(transport, loggerFactory)
    });