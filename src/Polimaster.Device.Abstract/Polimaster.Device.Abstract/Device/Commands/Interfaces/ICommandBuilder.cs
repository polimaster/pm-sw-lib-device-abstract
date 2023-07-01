using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Interfaces;

namespace Polimaster.Device.Abstract.Device.Commands.Interfaces;

public interface ICommandBuilder<TData> {
    ICommandBuilder<TData> With(ILoggerFactory? factory);
    ICommandBuilder<TData> With(ILogger? logger);

    ICommand<TValue, TData> Build<T, TValue>(IDevice<TData> device)
        where T : class, ICommand<TValue, TData>, new();
}