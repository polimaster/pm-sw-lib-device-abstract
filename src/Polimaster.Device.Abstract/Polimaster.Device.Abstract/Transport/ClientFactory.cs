using System;
using System.Linq;
using System.Reflection;

namespace Polimaster.Device.Abstract.Transport;

public class ClientFactory : IClientFactory {
    public T CreateClient<T, TConnectionParams>() where T : IClient<TConnectionParams> {
        var baseAssembly = typeof(T).GetTypeInfo().Assembly;
        var type = baseAssembly.DefinedTypes.FirstOrDefault(type => type.IsClass && type.ImplementedInterfaces.Any(inter => inter == typeof(T)));
        
        if (type == null) throw new TypeLoadException($"Cant find implementation for {typeof(T)}");
        if (type.GetConstructor(Type.EmptyTypes) == null) throw new TypeLoadException($"{type.FullName} should have parameterless constructor");
        
        return (T)(Activator.CreateInstance(type) ?? throw new InvalidOperationException($"Cant create implementation for {type.FullName}"));
    }
}