using System;
using Microsoft.Extensions.Logging;
using Moq;
using Polimaster.Device.Abstract.Device.Commands;

namespace Polimaster.Device.Abstract.Tests.Tests.Commands;

public class ALoggedTest : Mocks {
    private class TestLogged : ALogged {
        public TestLogged(ILoggerFactory? loggerFactory) : base(loggerFactory) { }
        public void TestLogDebug(string name) => LogDebug(name);
        public void TestLogError(Exception e, string name) => LogError(e, name);
    }

    [Fact]
    public void ShouldLogDebug() {
        var loggerMock = new Mock<ILogger>();
        var factoryMock = new Mock<ILoggerFactory>();
        factoryMock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(loggerMock.Object);

        var logged = new TestLogged(factoryMock.Object);
        logged.TestLogDebug("TestCmd");

        loggerMock.Verify(
            x => x.Log(
                LogLevel.Debug,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Execute TestCmd")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void ShouldLogError() {
        var loggerMock = new Mock<ILogger>();
        var factoryMock = new Mock<ILoggerFactory>();
        factoryMock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(loggerMock.Object);

        var logged = new TestLogged(factoryMock.Object);
        var ex = new Exception("TestEx");
        logged.TestLogError(ex, "TestCmd");

        loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error while executing TestCmd")),
                ex,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
