using System.ComponentModel;
using Polimaster.Device.Abstract.Helpers;

namespace Polimaster.Device.Abstract.Tests.Tests.Helpers;

public enum TestEnum {
    [Description("First Value")]
    First,
    Second
}

public class EnumExtensionsTest {
    [Fact]
    public void ShouldGetDescription() {
        var desc = TestEnum.First.GetDescription();
        Assert.Equal("First Value", desc);
    }

    [Fact]
    public void ShouldGetNullDescription() {
        var desc = TestEnum.Second.GetDescription();
        Assert.Null(desc);
    }

    [Fact]
    public void ShouldGetValues() {
        var values = EnumExtensions.GetValues<TestEnum>();
        Assert.Equal(2, values.Length);
        Assert.Contains(TestEnum.First, values);
        Assert.Contains(TestEnum.Second, values);
    }
}
