using System.ComponentModel.DataAnnotations;

namespace Polimaster.Device.Abstract.Tests.Impl.Settings;

public record MyParam {
    public int CommandPid { get; set; } = 1;
    
    [StringLength(10), Required]
    public string? Value { get; set; }
}