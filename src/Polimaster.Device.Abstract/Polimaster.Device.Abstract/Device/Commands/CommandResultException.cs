using System;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <inheritdoc />
public class CommandResultException(Exception exception) : Exception("Verifying command result failed", exception);