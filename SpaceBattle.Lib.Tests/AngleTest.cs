using System;
using StarWars.Lib;
using Xunit;

namespace StarWars.Test;

public class AngleTest
{
    [Fact]
    public void AdditionOfAngles_ShouldReturnNormalizedSum()
    {
        var a1 = new Angle(5);
        var a2 = new Angle(7);
        var result = a1 + a2;

        Assert.Equal(4, result.GetNumerator());
    }

    [Fact]
    public void EqualAngles_Equals_ReturnsTrue()
    {
        var a1 = new Angle(15);
        var a2 = new Angle(23);

        Assert.True(a1.Equals(a2));
    }

    [Fact]
    public void EqualAngles_OperatorEqual_ReturnsTrue()
    {
        var a1 = new Angle(15);
        var a2 = new Angle(23);

        Assert.True(a1 == a2);
    }

    [Fact]
    public void NotEqualAngles_Equals_ReturnsFalse()
    {
        var a1 = new Angle(1);
        var a2 = new Angle(2);

        Assert.False(a1.Equals(a2));
    }

    [Fact]
    public void NotEqualAngles_OperatorNotEqual_ReturnsTrue()
    {
        var a1 = new Angle(1);
        var a2 = new Angle(2);

        Assert.True(a1 != a2);
    }

    [Fact]
    public void Angle_HasHashCode()
    {
        var angle = new Angle(2);
        var hashCode = angle.GetHashCode();
        Assert.NotEqual(0, hashCode);
    }

    [Fact]
    public void NotEqualToNull()
    {
        var a1 = new Angle(1);
        Assert.False(a1.Equals(null));
    }

    [Fact]
    public void Angle_double()
    {
        Assert.IsType<double>(Math.Cos(new Angle(2)));
    }
}

