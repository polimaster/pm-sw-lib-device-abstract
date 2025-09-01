using System;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Device.Settings;

namespace Polimaster.Device.Abstract.Tests.Impl.Settings; 

public class HistoryIntervalSetting(IDataReader<TimeSpan> reader, ISettingDescriptor settingDescriptor, IDataWriter<TimeSpan>? writer = null)
    : ADeviceSetting<TimeSpan>(new SettingDefinition<TimeSpan> {
        Reader = reader,
        Descriptor = settingDescriptor,
        Writer = writer
    });