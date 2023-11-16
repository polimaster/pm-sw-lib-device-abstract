using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device.Commands;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands; 

public class StringWriteCommand : StringCommand<string> {
    protected override string Compile() {
        return "WRITE_DATA";
    }

    protected override string? Parse(string? value) {
        return value;
    }

    public override async Task Send(CancellationToken cancellationToken = new()) {
        throw new System.NotImplementedException();
    }
}