using Polimaster.Device.Abstract.Device.Settings;

namespace Polimaster.Device.Abstract.Tests.Impl.Settings;

public interface IMySettingDescriptors : ISettingDescriptors {
    public ISettingDescriptor StringSettingDescriptor { get; }
    public ISettingDescriptor MyParamSettingDescriptor { get; }
    public ISettingDescriptor HistoryIntervalSettingDescriptor { get; }
}