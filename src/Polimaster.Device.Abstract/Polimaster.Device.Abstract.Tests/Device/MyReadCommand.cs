using Polimaster.Device.Abstract.Transport.Commands;

namespace Polimaster.Device.Abstract.Tests.Device;

public class MyReadCommand : MyCommand, IReadCommand<string, MyParam, string> {
    public string? Result { get; private set; }
    public string Parse(string result) {
        Result = result;
        return result;
    }
}