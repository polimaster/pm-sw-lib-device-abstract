using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device;
using Polimaster.Device.Abstract.Device.Implementations;
using Polimaster.Device.Abstract.Device.Implementations.History;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;
using Polimaster.Device.Abstract.Tests.Impl.Device.Commands;
using Polimaster.Device.Abstract.Tests.Impl.Device.Settings;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device;

public interface IMyDevice : IHasBattery, IHasDose, IHasTemperatureSensor, IHasHistory<History> {
    IDeviceSetting<MyParam> MyParamSetting { get; }
    IDeviceSetting<string> StringSetting { get; }
}

public class History {
}

public class MyDevice : ADevice, IMyDevice {
    public IDeviceSetting<ushort?> HistoryInterval { get; }
    public IHistoryManager<History> HistoryManager { get; }
    public BatteryStatus? BatteryStatus { get; protected set; }

    public IDeviceSetting<MyParam> MyParamSetting { get; }
    public IDeviceSetting<string> StringSetting { get; }

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

        // building device commands and settings
        var paramReader = new MyParamReader(loggerFactory);
        var paramWriter = new MyParamWriter(loggerFactory);
        MyParamSetting = SettingBuilder.WithReader(paramReader).WithWriter(paramWriter).Build<MyParam>();

        var plainReader = new PlainReader(loggerFactory);
        var plainWriter = new PlainWriter(loggerFactory);
        StringSetting = SettingBuilder.WithReader(plainReader).WithWriter(plainWriter).Build<string>();
    }


    public override async Task<DeviceInfo?> ReadDeviceInfo(CancellationToken token = new()) {
        DeviceInfo = null;
        await Execute(async transport => { DeviceInfo = await transport.Read(_infoReader, token); });
        return DeviceInfo;
    }

    public async Task<BatteryStatus?> RefreshBatteryStatus(CancellationToken token = new()) {
        BatteryStatus = null;
        await Execute(async transport => { BatteryStatus = await transport.Read(_batteryStatusReader, token); });
        return BatteryStatus;
    }

    public async Task ResetDose(CancellationToken token = new()) {
        await Execute(async transport => { await transport.Exec(_resetDose, token); });
    }

    public async Task<double?> ReadTemperature(CancellationToken token = new()) {
        double? t = null;
        await Execute(async transport => { t = await transport.Read(_temperatureReader, token); });
        return t;
    }

    public async Task SetTime(CancellationToken token = new(), DateTime? dateTime = default) {
        var t = dateTime ?? DateTime.Now;
        await Execute(async transport => { await transport.Write(_timeWriter, t, token); });
    }

    public async Task<DateTime?> GetTime(CancellationToken token = new()) {
        DateTime? t = null;
        await Execute(async transport => { t = await transport.Read(_timeReader, token); });
        return t;
    }
}