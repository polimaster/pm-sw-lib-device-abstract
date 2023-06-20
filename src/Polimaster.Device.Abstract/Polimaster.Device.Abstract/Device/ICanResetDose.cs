using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device; 

/// <summary>
/// Identifies a device with reset dose function
/// </summary>
public interface ICanResetDose {
    Task ResetDose();
}