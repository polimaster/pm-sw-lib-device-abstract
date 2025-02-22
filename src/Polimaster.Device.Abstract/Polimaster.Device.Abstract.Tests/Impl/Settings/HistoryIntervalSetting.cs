﻿using System;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Device.Settings;

namespace Polimaster.Device.Abstract.Tests.Impl.Settings; 

public class HistoryIntervalSetting(IDataReader<TimeSpan> reader, IDataWriter<TimeSpan>? writer = null, ISettingBehaviour? settingBehaviour = null)
    : ADeviceSetting<TimeSpan>(reader, writer, settingBehaviour);