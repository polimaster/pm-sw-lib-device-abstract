using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device;
using Polimaster.Device.Abstract.Device.Implementations;
using Polimaster.Device.Abstract.Device.Implementations.History;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Tests.Impl.Commands;
using Polimaster.Device.Abstract.Tests.Impl.History;
using Polimaster.Device.Abstract.Tests.Impl.Settings;
using Polimaster.Device.Abstract.Tests.Impl.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device;

public interface IMyDevice : IDevice<IMyTransport, IMyDeviceStream>, IHasBattery, IHasDose, IHasTemperatureSensor, IHasHistory<HistoryRecord> {
    IDeviceSetting<MyParam> MyParamSetting { get; }
    IDeviceSetting<string> StringSetting { get; }
}

public class MyDevice : ADevice<IMyTransport, IMyDeviceStream>, IMyDevice {
    public IDeviceSetting<TimeSpan> HistoryInterval { get; }
    public IHistoryManager<HistoryRecord> HistoryManager { get; }
    public BatteryStatus? BatteryStatus { get; private set; }

    public IDeviceSetting<MyParam> MyParamSetting { get; }
    public IDeviceSetting<string> StringSetting { get; }

    private readonly DeviceInfoReader _infoReader;
    private readonly BatteryStatusReader _batteryStatusReader;
    private readonly ResetDose _resetDose;
    private readonly TemperatureReader _temperatureReader;
    private readonly TimeReader _timeReader;
    private readonly TimeWriter _timeWriter;

    public MyDevice(IMyTransport transport, ILoggerFactory? loggerFactory) : base(transport, loggerFactory) {
        _infoReader = new DeviceInfoReader(Transport, loggerFactory);
        _batteryStatusReader = new BatteryStatusReader(Transport, loggerFactory);
        _resetDose = new ResetDose(Transport, loggerFactory);
        _temperatureReader = new TemperatureReader(Transport, loggerFactory);
        _timeReader = new TimeReader(Transport, loggerFactory);
        _timeWriter = new TimeWriter(Transport, loggerFactory);

        var myParamBehaviour = new SettingDescriptorBase {
            AccessLevel = SettingAccessLevel.BASE,
            GroupName = "MyParamSettingGroup"
        };

        // building device commands and settings
        var paramReader = new MyParamReader(Transport, loggerFactory);
        var paramWriter = new MyParamWriter(Transport, loggerFactory);
        MyParamSetting = new MyParamSetting(paramReader, paramWriter, myParamBehaviour);


        var stringSettingBehaviour = new SettingDescriptorBase {
            AccessLevel = SettingAccessLevel.EXTENDED,
            GroupName = "StringSettingGroup"
        };
        var plainReader = new PlainReader(Transport, loggerFactory);
        var plainWriter = new PlainWriter(Transport, loggerFactory);
        StringSetting = new StringSetting(plainReader, plainWriter, stringSettingBehaviour);

        var historyIntervalBehaviour = new SettingDescriptorBase {
            AccessLevel = SettingAccessLevel.ADVANCED,
            GroupName = "Behaviour"
        };
        var intervalReader = new HistoryIntervalReader(Transport, loggerFactory);
        var intervalWriter = new HistoryIntervalWriter(Transport, loggerFactory);
        HistoryInterval = new HistoryIntervalSetting(intervalReader, intervalWriter, historyIntervalBehaviour);

        HistoryManager = new HistoryManager(Transport, loggerFactory);

    }


    public override async Task<DeviceInfo?> ReadDeviceInfo(CancellationToken token) {
        DeviceInfo = await _infoReader.Read(token);
        return DeviceInfo;
    }

    public async Task<BatteryStatus?> RefreshBatteryStatus(CancellationToken token) {
        BatteryStatus = await _batteryStatusReader.Read(token);
        return BatteryStatus;
    }

    public async Task ResetDose(CancellationToken token) => await _resetDose.Exec(token);

    public async Task<double?> ReadTemperature(CancellationToken token) => await _temperatureReader.Read(token);

    public async Task SetTime(CancellationToken token, DateTime? dateTime = null) {
        var t = dateTime ?? DateTime.Now;
        await _timeWriter.Write(t, token);
    }

    public async Task<DateTime?> GetTime(CancellationToken token) => await _timeReader.Read(token);
}