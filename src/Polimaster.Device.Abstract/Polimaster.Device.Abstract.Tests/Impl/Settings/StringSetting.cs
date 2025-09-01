using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Device.Settings;

namespace Polimaster.Device.Abstract.Tests.Impl.Settings; 

public class StringSetting(IDataReader<string> reader, IDataWriter<string>? writer = null, ISettingDescriptor? settingDescriptor = null)
    : ADeviceSetting<string>(reader, writer, settingDescriptor);