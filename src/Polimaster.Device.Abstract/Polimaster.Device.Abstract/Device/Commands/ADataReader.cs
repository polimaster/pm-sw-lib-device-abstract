﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Exceptions;
using Polimaster.Device.Abstract.Transport.Stream;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <summary>
/// Device data reader
/// </summary>
/// <typeparam name="T">Type of data to write</typeparam>
/// <typeparam name="TSteamData">Data type for device <see cref="IDeviceStream{T}"/></typeparam>
public abstract class ADataReader<T, TSteamData> : CommandBase<TSteamData>, IDataReader<T> {
    /// <inheritdoc />
    protected ADataReader(ILoggerFactory? loggerFactory) : base(loggerFactory) {
    }

    /// <summary>
    /// Compile command
    /// </summary>
    /// <returns></returns>
    /// <exception cref="CommandCompilationException"></exception>
    protected abstract TSteamData Compile();

    /// <summary>
    /// Parse data received from device
    /// </summary>
    /// <param name="res"></param>
    /// <returns></returns>
    /// <exception cref="CommandResultParsingException"></exception>
    protected abstract T Parse(TSteamData? res);

    /// <inheritdoc />
    public virtual async Task<T> Read<TStream>(TStream stream, CancellationToken cancellationToken) {
        var str = GetStream(stream);
        LogCommand(nameof(Read));

        try {
            await str.WriteAsync(Compile(), cancellationToken);
            var res = await str.ReadAsync(cancellationToken);
            return Parse(res);
        } catch (Exception e) {
            LogError(e, nameof(Read));
            throw;
        }
    }
}