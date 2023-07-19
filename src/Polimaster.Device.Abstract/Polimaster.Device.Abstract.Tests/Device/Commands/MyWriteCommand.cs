using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device.Commands;

namespace Polimaster.Device.Abstract.Tests.Device.Commands;

public class MyWriteCommand : StringCommand<MyParam> {

    public string CompiledCommand => Compile();

    protected override string Compile() {
        return $"{Value?.CommandPid} : {Value?.Value}";
    }

    protected override MyParam Parse(string? value) {
        throw new System.NotImplementedException();
    }

    public override async Task Send(CancellationToken cancellationToken = new CancellationToken()) {
        await Write(cancellationToken);
    }
}