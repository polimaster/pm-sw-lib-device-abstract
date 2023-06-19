using Polimaster.Device.Abstract.Commands;

namespace Polimaster.Device.Abstract.Tests.Device;

public class MyResultCommand : MyCommand, IResultCommand<string, MyParam, string> {
    public string? Result { get; private set; }
    public string Parse(string result) {
        Result = result;
        return result;
    }
}