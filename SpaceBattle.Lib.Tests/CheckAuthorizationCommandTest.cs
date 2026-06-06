using System;
using Moq;
using StarWars.Lib;
using Xunit;

namespace StarWars.Test;

public class CheckAuthorizationCommandTest
{
    [Fact]
    public void Execute_AuthorizedPlayer_DoesNotThrow()
    {
        var mockAuth = new Mock<IAuthorizationService>();
        mockAuth.Setup(x => x.IsAuthorized("game1", "ship1", "player1")).Returns(true);

        var cmd = new CheckAuthorizationCommand(mockAuth.Object, "game1", "ship1", "player1");

        cmd.Execute();

        mockAuth.Verify(x => x.IsAuthorized("game1", "ship1", "player1"), Times.Once);
    }

    [Fact]
    public void Execute_UnauthorizedPlayer_ThrowsUnauthorizedAccessException()
    {
        var mockAuth = new Mock<IAuthorizationService>();
        mockAuth.Setup(x => x.IsAuthorized("game1", "ship1", "hacker")).Returns(false);

        var cmd = new CheckAuthorizationCommand(mockAuth.Object, "game1", "ship1", "hacker");

        Assert.Throws<UnauthorizedAccessException>(() => cmd.Execute());
    }

    [Fact]
    public void Execute_AuthServiceThrows_Propagates()
    {
        var mockAuth = new Mock<IAuthorizationService>();
        mockAuth.Setup(x => x.IsAuthorized(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Throws<InvalidOperationException>();

        var cmd = new CheckAuthorizationCommand(mockAuth.Object, "g", "o", "p");

        Assert.Throws<InvalidOperationException>(() => cmd.Execute());
    }
}
