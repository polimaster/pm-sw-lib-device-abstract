using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device;
using Polimaster.Device.Abstract.Device.Implementations;
using Polimaster.Device.Abstract.Device.Implementations.History;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Tests.Impl.Device.Commands;
using Polimaster.Device.Abstract.Tests.Impl.Device.History;
using Polimaster.Device.Abstract.Tests.Impl.Device.Settings;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device;

public interface IMyDevice : IHasBattery, IHasDose, IHasTemperatureSensor, IHasHistory<HistoryRecord> {
    IDeviceSetting<MyParam?> MyParamSetting { get; }
    IDeviceSetting<string?> StringSetting { get; }
}

public class MyDevice : ADevice<string>, IMyDevice {
    public IDeviceSetting<ushort?> HistoryInterval { get; }
    public IHistoryManager<HistoryRecord> HistoryManager { get; }
    public BatteryStatus? BatteryStatus { get; private set; }

    public IDeviceSetting<MyParam?> MyParamSetting { get; }
    public IDeviceSetting<string?> StringSetting { get; }

    private readonly DeviceInfoReader _infoReader;
    private readonly BatteryStatusReader _batteryStatusReader;
    private readonly ResetDose _resetDose;
    private readonly TemperatureReader _temperatureReader;
    private readonly TimeReader _timeReader;
    private readonly TimeWriter _timeWriter;

    public MyDevice(ITransport<string> transport, ILoggerFactory? loggerFactory) : base(transport, loggerFactory) {
        _infoReader = new DeviceInfoReader(Transport, loggerFactory);
        _batteryStatusReader = new BatteryStatusReader(Transport, loggerFactory);
        _resetDose = new ResetDose(Transport, loggerFactory);
        _temperatureReader = new TemperatureReader(Transport, loggerFactory);
        _timeReader = new TimeReader(Transport, loggerFactory);
        _timeWriter = new TimeWriter(Transport, loggerFactory);

        var myParamBehaviour = new SettingBehaviourBase {
            AccessLevel = SettingAccessLevel.BASE,
            GroupName = "MyParamSettingGroup"
        };

        // building device commands and settings
        var paramReader = new MyParamReader(Transport, loggerFactory);
        var paramWriter = new MyParamWriter(Transport, loggerFactory);
        MyParamSetting = new MyParamSetting(paramReader, paramWriter, myParamBehaviour);


        var stringSettingBehaviour = new SettingBehaviourBase {
            AccessLevel = SettingAccessLevel.EXTENDED,
            GroupName = "StringSettingGroup"
        };
        var plainReader = new PlainReader(Transport, loggerFactory);
        var plainWriter = new PlainWriter(Transport, loggerFactory);
        StringSetting = new StringSetting(plainReader, plainWriter, stringSettingBehaviour);

        var historyIntervalBehaviour = new SettingBehaviourBase {
            AccessLevel = SettingAccessLevel.ADVANCED,
            GroupName = "Behaviour"
        };
        var intervalReader = new HistoryIntervalReader(Transport, loggerFactory);
        var intervalWriter = new HistoryIntervalWriter(Transport, loggerFactory);
        HistoryInterval = new HistoryIntervalSetting(intervalReader, intervalWriter, historyIntervalBehaviour);

        HistoryManager = new HistoryManager(Transport, loggerFactory);

    }


    public override async Task<DeviceInfo?> ReadDeviceInfo(CancellationToken token = new()) {
        DeviceInfo = await _infoReader.Read(token);
        return DeviceInfo;
    }

    public async Task<BatteryStatus?> RefreshBatteryStatus(CancellationToken token = new()) {
        BatteryStatus = await _batteryStatusReader.Read(token);
        return BatteryStatus;
    }

    public async Task ResetDose(CancellationToken token = new()) => await _resetDose.Exec(token);

    public async Task<double?> ReadTemperature(CancellationToken token = new()) => await _temperatureReader.Read(token);

    public async Task SetTime(CancellationToken token = new(), DateTime? dateTime = null) {
        var t = dateTime ?? DateTime.Now;
        await _timeWriter.Write(t, token);
    }

    public async Task<DateTime?> GetTime(CancellationToken token = new()) => await _timeReader.Read(token);
}