using Polimaster.Device.Abstract.Device.Settings;

namespace Polimaster.Device.Abstract.Tests.Tests.Settings;

public class SettingDescriptorTest {
    [Fact]
    public void ShouldBeEqual() {
        var d1 = new SettingDescriptor("Name", typeof(int), groupName: "G1");
        var d2 = new SettingDescriptor("Name", typeof(int), groupName: "G1");
        
        Assert.Equal(d1, d2);
        Assert.True(d1 == d2);
        Assert.Equal(d1.GetHashCode(), d2.GetHashCode());
    }

    [Fact]
    public void ShouldNotBeEqual() {
        var d1 = new SettingDescriptor("Name1", typeof(int));
        var d2 = new SettingDescriptor("Name2", typeof(int));
        var d3 = new SettingDescriptor("Name1", typeof(string));
        var d4 = new SettingDescriptor("Name1", typeof(int), groupName: "G2");

        Assert.NotEqual(d1, d2);
        Assert.NotEqual(d1, d3);
        Assert.NotEqual(d1, d4);
        Assert.True(d1 != d2);
    }

    [Fact]
    public void ShouldHandleProperties() {
        var range = new ValueRange { Min = 0, Max = 10, Step = 1 };
        var list = new object[] { 1, 2, 3 };
        var d = new SettingDescriptor("Name", typeof(int), 
            accessLevel: SettingAccessLevel.ADVANCED, 
            groupName: "Group", 
            description: "Desc", 
            unit: "Unit", 
            valueList: list, 
            valueRange: range);

        Assert.Equal("Name", d.Name);
        Assert.Equal(typeof(int), d.ValueType);
        Assert.Equal(SettingAccessLevel.ADVANCED, d.AccessLevel);
        Assert.Equal("Group", d.GroupName);
        Assert.Equal("Desc", d.Description);
        Assert.Equal("Unit", d.Unit);
        Assert.Equal(list, d.ValueList);
        Assert.Equal(range, d.Range);
        Assert.NotEqual(0, d.Id);
    }

    [Fact]
    public void IdShouldBeStable() {
        var d = new SettingDescriptor("Name", typeof(int), groupName: "G1");
        var id1 = d.Id;
        var id2 = d.Id;
        Assert.Equal(id1, id2);
    }

    [Fact]
    public void IdShouldBeDifferentForDifferentFields() {
        var d1 = new SettingDescriptor("Name1", typeof(int));
        var d2 = new SettingDescriptor("Name2", typeof(int));
        Assert.NotEqual(d1.Id, d2.Id);
    }
}
