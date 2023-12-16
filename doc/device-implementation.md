### Define device interface and class

```c#
public interface IPm1703 : IDevice, 
    IHasBattery {
    IDeviceSetting<ushort?> HistoryInterval { get; }
}


public class Pm1703 : ADevice, IPm1703 {
    public BatteryStatus BatteryStatus { get; private set; }
    public IDeviceSetting<ushort?> HistoryInterval { get; private set; } = null!;

    public override async Task<DeviceInfo> ReadDeviceInfo(CancellationToken cancellationToken = new()) {
        var readSerialNumber = CommandBuilder.Build<SerialNumberRead>();
        await readSerialNumber.Send(cancellationToken);

        var readFirmwareVersion = CommandBuilder.Build<FirmwareVersionRead>();
        await readFirmwareVersion.Send(cancellationToken);

        DeviceInfo = new DeviceInfo {
            Id = readFirmwareVersion.Value,
            Serial = readSerialNumber.Value,
            Model = "PM1703"
        };
        return DeviceInfo;
    }

    public override void BuildSettings() {
    
        var historyIntervalReadCommand = CommandBuilder.Build<HistoryIntervalRead>();
        var historyIntervalWriteCommand = CommandBuilder.Build<HistoryIntervalWrite>();
        HistoryInterval = SettingBuilder
            .WithReadCommand(historyIntervalReadCommand)
            .WithWriteCommand(historyIntervalWriteCommand)
            .Build<ushort?>();
    }

    public async Task SetTime(CancellationToken cancellationToken = new(), DateTime? dateTime = default) {
        var c = CommandBuilder.Build<TimeWrite>();
        c.Value = dateTime ?? DateTime.Now;
        await c.Send(cancellationToken);
    }

    public async Task<DateTime?> GetTime(CancellationToken cancellationToken = new()) {
        var c = CommandBuilder.Build<TimeRead>();
        await c.Send(cancellationToken);
        return c.Value;
    }

    public async Task<BatteryStatus> RefreshBatteryStatus(CancellationToken cancellationToken) {
        var voltsCommand = CommandBuilder.Build<BatteryStatusRead>();
        await voltsCommand.Send(cancellationToken);

        var batThCommand = CommandBuilder.Build<BatteryThresholdRead>();
        await batThCommand.Send(cancellationToken);

        BatteryStatus = new BatteryStatus {
            Volts = voltsCommand.Value,
            Percents = (voltsCommand.Value - batThCommand.Value) / ((1.5 - batThCommand.Value) / 100)
        };
        return BatteryStatus;
    }

}

```