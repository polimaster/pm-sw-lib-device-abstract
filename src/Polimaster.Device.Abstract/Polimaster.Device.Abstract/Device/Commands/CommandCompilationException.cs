﻿using System;

namespace Polimaster.Device.Abstract.Device.Commands; 

/// <summary>
/// Rises while result of <see cref="AWriteCommand{T,TTransport}.Compile"/> fails
/// </summary>
public class CommandCompilationException : Exception{
    public CommandCompilationException(string message) : base(message) {
    }
}