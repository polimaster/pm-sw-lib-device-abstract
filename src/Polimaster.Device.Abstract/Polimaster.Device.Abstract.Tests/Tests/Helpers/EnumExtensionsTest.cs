using System.ComponentModel;
using Polimaster.Device.Abstract.Helpers;

namespace Polimaster.Device.Abstract.Tests.Tests.Helpers;

public enum TestEnum {
    [Description("First Value")]
    FIRST,
    SECOND
}

public class EnumExtensionsTest {
    [Fact]
    public void ShouldGetDescription() {
        var desc = TestEnum.FIRST.GetDescription();
        Assert.Equal("First Value", desc);
    }

    [Fact]
    public void ShouldGetNullDescription() {
        var desc = TestEnum.SECOND.GetDescription();
        Assert.Null(desc);
    }

    [Fact]
    public void ShouldGetValues() {
        var values = EnumExtensions.GetValues<TestEnum>();
        Assert.Equal(2, values.Length);
        Assert.Contains(TestEnum.FIRST, values);
        Assert.Contains(TestEnum.SECOND, values);
    }
}
