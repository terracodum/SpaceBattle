using System;
using Moq;
using StarWars.Lib;
using Xunit;

namespace StarWars.Test;

public class RotateCommandTest
{
    [Fact]
    public void Rotate_MovesObjectByAngularVelocity()
    {
        var rotating = new Mock<IRotatingObject>();

        rotating.Setup(x => x.GetAngle()).Returns(new Angle(1));
        rotating.Setup(m => m.GetAngularVelocity()).Returns(new Angle(1));
        rotating.Setup(x => x.SetAngle(It.IsAny<Angle>()));

        var cmd = new RotateCommand(rotating.Object);
        cmd.Execute();
        rotating.Verify(x => x.SetAngle(new Angle(2)), Times.Once);
    }

    [Fact]
    public void Rotate_WhenCannotGetAngle_ThrowsException()
    {
        var rotating = new Mock<IRotatingObject>();

        rotating.Setup(x => x.GetAngle()).Throws(new ArgumentException());
        rotating.Setup(m => m.GetAngularVelocity()).Returns(new Angle(1));

        var cmd = new RotateCommand(rotating.Object);
        Assert.Throws<ArgumentException>(() => cmd.Execute());
    }

    [Fact]
    public void Rotate_WhenCannotGetAngularVelocity_ThrowsException()
    {
        var rotating = new Mock<IRotatingObject>();

        rotating.Setup(x => x.GetAngle()).Returns(new Angle(1));
        rotating.Setup(m => m.GetAngularVelocity()).Throws(new ArgumentException());

        var cmd = new RotateCommand(rotating.Object);
        Assert.Throws<ArgumentException>(() => cmd.Execute());
    }

    [Fact]
    public void Rotate_WhenCannotSetAngle_ThrowsException()
    {
        var rotating = new Mock<IRotatingObject>();

        rotating.Setup(x => x.GetAngle()).Returns(new Angle(1));
        rotating.Setup(m => m.GetAngularVelocity()).Returns(new Angle(1));
        rotating.Setup(x => x.SetAngle(new Angle(2))).Throws(new ArgumentException());

        var cmd = new RotateCommand(rotating.Object);
        Assert.Throws<ArgumentException>(() => cmd.Execute());
    }
}
