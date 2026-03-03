using System;
using Polimaster.Device.Abstract.Helpers;

namespace Polimaster.Device.Abstract.Tests.Tests.Helpers;

public class TypeExtensionsTest {
    [Theory]
    [InlineData(typeof(int), true)]
    [InlineData(typeof(string), true)]
    [InlineData(typeof(decimal), true)]
    [InlineData(typeof(DateTime), true)]
    [InlineData(typeof(DateTimeOffset), true)]
    [InlineData(typeof(TimeSpan), true)]
    [InlineData(typeof(Guid), true)]
    [InlineData(typeof(DayOfWeek), true)]
    [InlineData(typeof(int?), true)]
    [InlineData(typeof(DayOfWeek?), true)]
    [InlineData(typeof(object), false)]
    [InlineData(typeof(TypeExtensionsTest), false)]
    public void ShouldDetectSimpleType(Type type, bool expected) {
        Assert.Equal(expected, type.IsSimpleType());
    }
}
