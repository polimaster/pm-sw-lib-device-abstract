using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Commands;

/// <summary>
/// Command for device with command result returned.
/// </summary>
/// <typeparam name="TParseResult">Type of command result</typeparam>
/// <typeparam name="TCompiled"><see cref="ICommand{TParam,TCompiled}"/></typeparam>
public interface IResultCommand<out TParseResult, TCompiled> : ICommand<TCompiled> {
    
    /// <summary>
    /// This method should parse result of command or throws exception,
    /// </summary>s
    /// <param name="result">
    /// <see cref="ITransport{TData,TConnectionParams}.Read"/>
    /// Result of executed command
    /// </param>
    /// <exception cref="CommandResultParsingException"></exception>
    TParseResult? Parse(TCompiled result);
}

/// <summary>
/// Parametrized command for device with command result returned.
/// </summary>
/// <typeparam name="TParseResult">Type of command result</typeparam>
/// <typeparam name="TParam"><see cref="ICommand{TParam,TCompiled}"/></typeparam>
/// <typeparam name="TCompiled"><see cref="ICommand{TParam,TCompiled}"/></typeparam>
public interface IResultCommand<out TParseResult, TParam, TCompiled> : IResultCommand<TParseResult, TCompiled>, ICommand<TParam, TCompiled> {
}
