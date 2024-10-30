using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Device.Settings;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Settings; 

public class StringSetting(IDataReader<string?> reader, IDataWriter<string?>? writer = null, string? groupName = null)
    : DeviceSettingBase<string?>(reader, writer, groupName);