using Polimaster.Device.Abstract;

namespace Polimaster.Device.Transport.Win.IrDA;

/// <summary>
/// IrDA device connection properties
/// </summary>
public struct IrDaDevice : IStringify {
    
    /// <summary>
    /// Device identifier
    /// </summary>
    public string Name;

    /// <inheritdoc />
    public override string ToString() {
        return Name;
    }
}