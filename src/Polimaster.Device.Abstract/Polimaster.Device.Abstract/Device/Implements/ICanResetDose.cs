using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Implements; 

/// <summary>
/// Identifies a device with reset dose function
/// </summary>
public interface ICanResetDose {
    
    /// <summary>
    /// Resets accumulated dose on device
    /// </summary>
    /// <returns></returns>
    Task ResetDose();
}