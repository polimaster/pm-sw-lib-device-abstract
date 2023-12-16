### Define device commands

This library currently has two abstract command implementations: StringCommand and ByteCommand.
You can use one of them according to device transport support. Here is an example using StringCommand.

```c#
public class HistoryIntervalRead : StringCommand<ushort?> {
    protected override byte[] Compile() => "HInterval?";

    protected override ushort? Parse(string? result) {
        if (result != null) return Convert.ToUint16(result);
        throw new NullReferenceException();
    }
    
    public override async Task Send(CancellationToken cancellationToken = new()) {
        await Read(cancellationToken);
    }
}
```


### Define device settings

Device setup is a wrapper for read/write commands. For example, if you want to configure the HistoryInterval setting for a device, both HistoryIntervalRead and HistoryIntervalWrite must be implemented. These commands are then used in the device's [BuildSettings() metod](./Device-implementation) to build the HistoryInterval setting.

```c#
var historyIntervalReadCommand = CommandBuilder.Build<HistoryIntervalRead>();
var historyIntervalWriteCommand = CommandBuilder.Build<HistoryIntervalWrite>();
HistoryInterval = SettingBuilder
    .WithReadCommand(historyIntervalReadCommand)
    .WithWriteCommand(historyIntervalWriteCommand)
    .Build<ushort?>();
```