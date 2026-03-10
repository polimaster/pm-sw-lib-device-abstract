using System.Collections.Generic;
using System.ComponentModel;
using Moq;
using Polimaster.Device.Abstract.Device.Settings;

namespace Polimaster.Device.Abstract.Tests.Tests.Settings;

public class ADeviceSettingProxyPropertyChangedTest {
    private class TestProxy : ADeviceSettingProxy<string, string> {
        public TestProxy(IDeviceSetting<string> proxiedSetting) 
            : base(proxiedSetting, new SettingDescriptor("Test", typeof(string))) {
        }

        protected override string? GetProxied() => ProxiedSetting.Value;

        protected override string CreateNewProxiedValue(string proxied, string value) => value;
    }

    [Theory]
    [InlineData(nameof(IDeviceSetting.IsDirty))]
    [InlineData(nameof(IDeviceSetting.HasValue))]
    [InlineData(nameof(IDeviceSetting.IsSynchronized))]
    [InlineData(nameof(IDeviceSetting.IsError))]
    [InlineData(nameof(IDeviceSetting.Exception))]
    [InlineData(nameof(IDeviceSetting.IsValid))]
    [InlineData(nameof(IDeviceSetting.ValidationResults))]
    public void ShouldProxyPropertyChanged(string propertyName) {
        var mock = new Mock<IDeviceSetting<string>>();
        var proxy = new TestProxy(mock.Object);
        var raisedProperties = new List<string>();
        proxy.PropertyChanged += (_, args) => raisedProperties.Add(args.PropertyName);

        mock.Raise(m => m.PropertyChanged += null, new PropertyChangedEventArgs(propertyName));

        Assert.Contains(propertyName, raisedProperties);
    }

    [Fact]
    public void ShouldProxyValueAndUntypedValue() {
        var mock = new Mock<IDeviceSetting<string>>();
        mock.Setup(m => m.Value).Returns("new value");
        var proxy = new TestProxy(mock.Object);
        var raisedProperties = new List<string>();
        proxy.PropertyChanged += (_, args) => raisedProperties.Add(args.PropertyName);

        mock.Raise(m => m.PropertyChanged += null, new PropertyChangedEventArgs(nameof(IDeviceSetting<string>.Value)));

        Assert.Contains(nameof(IDeviceSetting<string>.Value), raisedProperties);
        Assert.Contains(nameof(IDeviceSetting.UntypedValue), raisedProperties);
    }
}
