using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Device.Settings;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Settings; 

public class HistoryIntervalSetting(IDataReader<ushort?> reader, IDataWriter<ushort?>? writer = null)
    : DeviceSettingBase<ushort?>(reader, writer);