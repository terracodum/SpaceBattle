using System;
using Moq;
using StarWars.Lib;
using Xunit;

namespace StarWars.Test;

public class MoveClassTest
{
    [Fact]
    public void MoveCommandTest()
    {
        var moving = new Mock<IMovingObject>();

        moving.Setup(x => x.GetPosition()).Returns(new Vector([12, 5]));
        moving.Setup(m => m.GetVelocity()).Returns(new Vector([-4, 1]));
        moving.Setup(x => x.SetPosition(It.IsAny<Vector>()));

        var cmd = new MoveCommand(moving.Object);
        cmd.Execute();
        moving.Verify(x => x.SetPosition(new Vector(new int[] { 8, 6 })), Times.Once);
    }

    [Fact]
    public void MoveCommandNoPositionTest()
    {
        var moving = new Mock<IMovingObject>();

        moving.Setup(m => m.GetPosition()).Throws(new ArgumentException());
        moving.Setup(m => m.GetVelocity()).Returns(new Vector([-4, 1]));

        var cmd = new MoveCommand(moving.Object);

        Assert.Throws<ArgumentException>(() => cmd.Execute());
    }

    [Fact]
    public void MoveCommandNoVelocityTest()
    {
        var moving = new Mock<IMovingObject>();

        moving.Setup(m => m.GetPosition()).Returns(new Vector([12, 5]));
        moving.Setup(m => m.GetVelocity()).Throws(new ArgumentException());

        var cmd = new MoveCommand(moving.Object);

        Assert.Throws<ArgumentException>(() => cmd.Execute());
    }

    [Fact]
    public void MoveCommandCannotSetPositionTest()
    {
        var moving = new Mock<IMovingObject>();
        moving.Setup(x => x.GetPosition()).Returns(new Vector([12, 5]));
        moving.Setup(x => x.GetVelocity()).Returns(new Vector([-4, 1]));
        moving.Setup(x => x.SetPosition(It.IsAny<Vector>())).Throws<Exception>();

        var cmd = new MoveCommand(moving.Object);
        Assert.Throws<Exception>(() => cmd.Execute());
    }
}
