namespace Polimaster.Device.Abstract.Device.Implementations.Status;

/// <summary>
/// Device has readable online status
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IHasStatus<T> {
    /// <summary>
    /// See <see cref="IDeviceStatus{T}"/>
    /// </summary>
    IDeviceStatus<T> Status { get; }
}