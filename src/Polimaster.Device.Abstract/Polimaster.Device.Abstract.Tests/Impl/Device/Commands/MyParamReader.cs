using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Device.Commands.Exceptions;
using Polimaster.Device.Abstract.Tests.Impl.Device.Settings;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands; 

public class MyParamReader(ITransport transport, ILoggerFactory? loggerFactory = null) : ADataReader<MyParam?>(transport, loggerFactory) {
    protected override byte[] Compile() => Encoding.UTF8.GetBytes($"{Cmd.PREFIX}{Cmd.QUESTION_MARK}");

    protected override MyParam? Parse(byte[] data) {

        var str = Encoding.UTF8.GetString(data);
        
        try {
            // assume data format = "CMD=123:456"
            var res = new MyParam();
            var split = str.Split('=');
            var d = split[1].Split(':');
            res.CommandPid = Convert.ToInt32(d[0]);
            res.Value = d[1];
        } catch(Exception e) {
            throw new CommandResultParsingException(e);
        }

        return null;
    }
}