using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Device.Settings;

namespace Polimaster.Device.Abstract.Tests.Impl.Settings; 

public class HistoryIntervalSetting(IDataReader<ushort> reader, IDataWriter<ushort>? writer = null, ISettingBehaviour? settingBehaviour = null)
    : DeviceSettingBase<ushort>(reader, writer, settingBehaviour);