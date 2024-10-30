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

public class MyDevice : ADevice, IMyDevice {
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

    public MyDevice(ITransport transport, ILoggerFactory? loggerFactory) : base(transport, loggerFactory) {
        _infoReader = new DeviceInfoReader(loggerFactory);
        _batteryStatusReader = new BatteryStatusReader(loggerFactory);
        _resetDose = new ResetDose(loggerFactory);
        _temperatureReader = new TemperatureReader(loggerFactory);
        _timeReader = new TimeReader(loggerFactory);
        _timeWriter = new TimeWriter(loggerFactory);

        var myParamBehaviour = new SettingBehaviourBase(SettingAccessLevel.BASE, "MyParamSettingGroup");

        // building device commands and settings
        var paramReader = new MyParamReader(loggerFactory);
        var paramWriter = new MyParamWriter(loggerFactory);
        MyParamSetting = new MyParamSetting(paramReader, paramWriter, myParamBehaviour);


        var stringSettingBehaviour = new SettingBehaviourBase(SettingAccessLevel.EXTENDED, "StringSettingGroup");
        var plainReader = new PlainReader(loggerFactory);
        var plainWriter = new PlainWriter(loggerFactory);
        StringSetting = new StringSetting(plainReader, plainWriter, stringSettingBehaviour);

        var historyIntervalBehaviour = new SettingBehaviourBase(SettingAccessLevel.ADVANCED, "Behaviour");
        var intervalReader = new HistoryIntervalReader(loggerFactory);
        var intervalWriter = new HistoryIntervalWriter(loggerFactory);
        HistoryInterval = new HistoryIntervalSetting(intervalReader, intervalWriter, historyIntervalBehaviour);

        HistoryManager = new HistoryManager(loggerFactory);

    }


    public override async Task<DeviceInfo?> ReadDeviceInfo(CancellationToken token = new()) {
        DeviceInfo = null;
        await Execute(async transport => { DeviceInfo = await transport.Read(_infoReader, token); }, token);
        return DeviceInfo;
    }

    public async Task<BatteryStatus?> RefreshBatteryStatus(CancellationToken token = new()) {
        BatteryStatus = null;
        await Execute(async transport => { BatteryStatus = await transport.Read(_batteryStatusReader, token); }, token);
        return BatteryStatus;
    }

    public async Task ResetDose(CancellationToken token = new()) {
        await Execute(async transport => { await transport.Exec(_resetDose, token); }, token);
    }

    public async Task<double?> ReadTemperature(CancellationToken token = new()) {
        double? t = null;
        await Execute(async transport => { t = await transport.Read(_temperatureReader, token); }, token);
        return t;
    }

    public async Task SetTime(CancellationToken token = new(), DateTime? dateTime = default) {
        var t = dateTime ?? DateTime.Now;
        await Execute(async transport => { await transport.Write(_timeWriter, t, token); }, token);
    }

    public async Task<DateTime?> GetTime(CancellationToken token = new()) {
        DateTime? t = null;
        await Execute(async transport => { t = await transport.Read(_timeReader, token); }, token);
        return t;
    }
}