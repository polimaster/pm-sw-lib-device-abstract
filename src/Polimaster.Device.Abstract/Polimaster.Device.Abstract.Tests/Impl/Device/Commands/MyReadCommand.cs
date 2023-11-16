using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Tests.Impl.Device.Settings;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Commands; 

public class MyReadCommand : StringCommand<MyParam> {
    protected override string Compile() {
        return $"{Value?.CommandPid} : {Value?.Value}";
    }

    protected override MyParam? Parse(string? data) {
        Value ??= new MyParam();
        Value.Value = data;
        return Value;
    }

    public override async Task Send(CancellationToken cancellationToken = new CancellationToken()) {
        await Read(cancellationToken);
    }
}