using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Exceptions;
using Polimaster.Device.Abstract.Tests.Impl.Settings;
using Polimaster.Device.Abstract.Tests.Impl.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Commands; 

public class MyParamReader(IMyTransport transport, ILoggerFactory? loggerFactory = null) : MyDeviceStreamReader<MyParam>(transport, loggerFactory) {
    protected override byte[] Compile() => Encoding.UTF8.GetBytes($"{Cmd.PREFIX}{Cmd.QUESTION_MARK}");

    protected override MyParam Parse(byte[]? data) {
        ArgumentNullException.ThrowIfNull(data);

        var str = Encoding.UTF8.GetString(data);
        try {
            // assume data format = "CMD=123:456"
            var res = new MyParam();
            var split = str.Split('=');
            var d = split[1].Split(':');
            res.CommandPid = Convert.ToInt32(d[0]);
            res.Value = d[1];
            return res;
        } catch(Exception e) {
            throw new CommandResultParsingException(e);
        }
    }
}