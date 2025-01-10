using System;
using Polimaster.Device.Abstract.Tests.Impl.Device.Transport;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Tests.Transport;

public class MyClientTest : Mocks {
    private readonly ClientParams _param = new(123456, 80);
    
    // [Fact]
    // public void ShouldReturnStream() {
    //     var client = new MyClient(_param, LOGGER_FACTORY);
    //
    //     client.Open();
    //     var stream = client.GetStream();
    //
    //     Assert.True(stream is MyDeviceStream);
    //     Assert.True(client.Connected);
    // }

    [Fact]
    public void ShouldTrowExceptionOnGetStream() {
        var client = new MyClient(_param, LOGGER_FACTORY);
        client.Close();

        Exception? exception = null;
        
        try {
            client.GetStream();
        } catch (Exception e) {
            exception = e;
        }
        
        Assert.NotNull(exception);
        Assert.True(exception.GetType() == typeof(DeviceClientException));
        Assert.False(client.Connected);
    }

    // [Fact]
    // public void ShouldCloseConnection() {
    //     var client = new MyClient(_param, LOGGER_FACTORY);
    //     
    //     client.Close();
    //     
    //     client.Open();
    //     Assert.True(client.Connected);
    //     
    //     client.Close();
    //     Assert.False(client.Connected);
    // }
}