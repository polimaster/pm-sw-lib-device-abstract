using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Device.Settings;

namespace Polimaster.Device.Abstract.Tests.Impl.Settings; 

public class StringSetting(IDataReader<string> reader, ISettingDescriptor settingDescriptor, IDataWriter<string>? writer = null)
    : ADeviceSetting<string>(new SettingDefinition<string> {
        Reader = reader,
        Writer = writer,
        Descriptor = settingDescriptor
    });