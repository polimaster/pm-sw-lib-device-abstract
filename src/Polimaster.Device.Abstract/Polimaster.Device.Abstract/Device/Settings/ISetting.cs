using System;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Settings; 

public interface ISetting<T> {
    T? Value { get; set; }
    bool IsDirty { get; }
    bool IsError { get; }
    Exception? Exception { get; }

    Task<T> Read();
    Task CommitChanges();
}


public abstract class ASetting<T> : ISetting<T> {
    // private readonly IDevice<TData, TConnectionParams> _device;

    private T? _value;
    private readonly object _lock = new();

    public T? Value {
        get {
            lock (_lock) {
                if (_value == null) {
                    _value = Read().Result;
                    IsDirty = false;
                }
                return _value;    
            }
        }
        set {
            if (value != null && value.Equals(_value)) return;
            IsDirty = true;
            _value = value;
        }
    }


    public bool IsDirty { get; private set; }

    public bool IsError => Exception != null;
    public Exception? Exception { get; }
    
    

    // protected ASetting(IDevice<TData, TConnectionParams> device) {
    //     _device = device;
    // }

    public abstract Task<T> Read();
    public abstract Task CommitChanges();
}