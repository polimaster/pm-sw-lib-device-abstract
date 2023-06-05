using Polimaster.Device.Abstract.Transport.Commands;

namespace Polimaster.Device.Abstract.Tests.Device;

public class MyCommand : ICommand<MyParam, string> {
    public MyParam Param { get; set; }
    public string Compile() {
        return $"{Param.CommandPid} : {Param.Value}";
    }
}