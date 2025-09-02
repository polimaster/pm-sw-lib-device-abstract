using System;
using System.Collections.Generic;
using Polimaster.Device.Abstract.Device.Settings;

namespace Polimaster.Device.Abstract.Tests.Impl.Settings;

public class MySettingDescriptors : IMySettingDescriptors {
    public IEnumerable<ISettingDescriptor> GetAll() {
        return [
            StringSettingDescriptor,
            MyParamSettingDescriptor,
            HistoryIntervalSettingDescriptor
        ];
    }

    public ISettingDescriptor StringSettingDescriptor { get; } =
        new SettingDescriptor("test1", typeof(string), SettingAccessLevel.EXTENDED, "StringSettingGroup");

    public ISettingDescriptor MyParamSettingDescriptor { get; } =
        new SettingDescriptor("test", typeof(MyParam), SettingAccessLevel.BASE, "MyParamSettingGroup");

    public ISettingDescriptor HistoryIntervalSettingDescriptor { get; } =
        new SettingDescriptor("test2", typeof(TimeSpan), SettingAccessLevel.ADVANCED, "Behaviour");
}