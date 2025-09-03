using System;
using System.Collections.Generic;
using Polimaster.Device.Abstract.Device.Settings;

namespace Polimaster.Device.Abstract.Tests.Impl.Settings;

public readonly struct MySettingDescriptors : IMySettingDescriptors {
    public const string STRING_SETTING_GROUP = "StringSettingGroup";
    public const string MY_PARAM_SETTING_GROUP = "MyParamSettingGroup";
    public const string BEHAVIOUR_SETTING_GROUP = "Behaviour";

    public MySettingDescriptors() {
    }

    public IEnumerable<ISettingDescriptor> GetAll() {
        return [
            StringSettingDescriptor,
            MyParamSettingDescriptor,
            HistoryIntervalSettingDescriptor
        ];
    }

    public ISettingDescriptor StringSettingDescriptor { get; } =
        new SettingDescriptor("test1", typeof(string), SettingAccessLevel.EXTENDED, STRING_SETTING_GROUP);

    public ISettingDescriptor MyParamSettingDescriptor { get; } =
        new SettingDescriptor("test", typeof(MyParam), SettingAccessLevel.BASE, MY_PARAM_SETTING_GROUP);

    public ISettingDescriptor HistoryIntervalSettingDescriptor { get; } =
        new SettingDescriptor("test2", typeof(TimeSpan), SettingAccessLevel.ADVANCED, BEHAVIOUR_SETTING_GROUP);
}