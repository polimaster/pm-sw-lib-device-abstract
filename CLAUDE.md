# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

All commands run from `src/Polimaster.Device.Abstract/`:

```bash
# Build
dotnet build

# Run all tests
dotnet test

# Run a single test class
dotnet test --filter "FullyQualifiedName~SettingsTest"

# Run a single test method
dotnet test --filter "FullyQualifiedName~SettingsTest.ShouldReadSetting"

# Pack NuGet package
dotnet pack Polimaster.Device.Abstract/Polimaster.Device.Abstract.csproj -c Release
```

## Architecture

This is a `netstandard2.0` abstractions library for communicating with Polimaster hardware devices. It defines the layers that concrete device libraries (e.g., PM1703) must implement.

### Layer Stack (bottom to top)

**1. Client** (`IClient<TStream>` / `AClient<TStream>`)
- Wraps the physical connection (USB serial port, IrDA socket, etc.)
- Exposes `Open()`, `Close()`, `GetStream()`, `Connected`, `ConnectionId`
- `TStream` is the stream type the client produces (e.g., `Stream`, `Socket`)

**2. Transport** (`ITransport<TStream>` / `ATransport<TStream>`)
- Wraps a `IClient<TStream>`; handles thread-safe stream access via `SemaphoreSlim`
- Key overridable properties: `SyncStreamAccess` (default `true`), `KeepOpen` (default `false`), `Sleep` (ms between commands)
- `ExecOnStream(action, token)` opens connection, runs the action, and auto-retries once on failure

**3. Commands** (`ICommand` / `ACommand<TStream>`)
- Each command encapsulates one device operation
- `AReader<T, TStream>` implements `IDataReader<T>` — reads a value from the stream
- `AWriter<T, TStream>` implements `IDataWriter<T>` — writes a value to the stream
- Both inherit from `ACommand<TStream>` which calls `Transport.ExecOnStream`

**4. Settings** (`IDeviceSetting<T>` / `ADeviceSetting<T>`)
- A setting wraps a read command (`IDataReader<T>`) and optional write command (`IDataWriter<T>`)
- Built via `SettingDefinition<T>` (holds descriptor, reader, writer)
- Key state flags: `IsSynchronized`, `IsDirty`, `IsValid`, `IsError`, `ReadOnly`
- `Read()` — skips if already synchronized; `Reset()` — forces re-read from device
- `CommitChanges()` — writes to device only if `IsDirty && IsValid`
- `ADeviceSettingProxy<T>` delegates to another `IDeviceSetting<T>`, used for settings shared across device modes
- Settings implement `INotifyPropertyChanged`

**5. Device** (`IDevice` / `IDevice<TTransport,TStream>` / `ADevice<TTransport,TStream>`)
- Holds a `TTransport` and a collection of `IDeviceSetting<>` properties
- `ReadAllSettings()` / `WriteAllSettings()` iterate all `IDeviceSetting<>` properties via reflection and invoke `Read`/`CommitChanges`
- `GetSettings()` returns only settings whose `Descriptor` is in the injected `ISettingDescriptors`
- `ReadDeviceInfo()` is abstract — concrete device must implement
- Fires `IsDisposing` event and disposes transport on `Dispose()`

**6. Discovery** (`ITransportDiscovery<TConnectionParams>` / `ATransportDiscovery<TConnectionParams>`)
- Runs a background `Task.Run` loop calling abstract `Search()` every `Sleep` ms
- Fires `Found(IEnumerable<TConnectionParams>)` and `Lost(IEnumerable<TConnectionParams>)` events

**7. Device Manager** (`IDeviceManager<T>` / `ADeviceManager<T>`)
- Subscribes to `Discovery.Found`/`Lost` events
- `OnFound`: calls abstract `CreateClient(params)` → `CreateTransport(client)` → `CreateDevice(transport)`, fires `Attached`
- `OnLost`: finds matching devices via `HasSame(transport)`, fires `Removed`, disposes them
- Thread-safe device list protected by `DevicesRaceLock`

### Setting Descriptors

`ISettingDescriptor` / `SettingDescriptor` identify a setting by name and value type. `ISettingDescriptors` is a collection injected into `ADevice` to filter which `IDeviceSetting<>` properties are exposed via `GetSettings()`. Concrete device libraries define their own descriptor implementations.

### Test Project

Tests live in `Polimaster.Device.Abstract.Tests/`. The `Impl/` folder contains concrete test implementations of all abstract classes. The `Tests/` folder contains xunit test classes. The test project targets `net9.0`.
