using System;
using System.Linq;
using StarWars.Lib;
using Xunit;

namespace SpaceBattle.Test;

public class VectorTests
{
    [Fact]
    public void VecorIsInitializableTest()
    {
        int[] elements = { 1, 2, 3 };
        var vector = new Vector(elements);
        Assert.True(elements.SequenceEqual(vector.GetElements()));
    }

    [Fact]
    public void VectorAdditionTest()
    {
        var v1 = new Vector([1, -1, 2]);
        var v2 = new Vector([-1, 1, -2]);
        Assert.Equal(new Vector([0, 0, 0]), v1 + v2);
    }

    [Fact]
    public void VectorAdditionDifferentLengthFirstLongerTest()
    {
        var v1 = new Vector([1, 2, 3]);
        var v2 = new Vector([1, 2]);
        Assert.Throws<ArgumentException>(() => _ = v1 + v2);
    }

    [Fact]
    public void VectorAdditionDifferentLengthSecondLongerTest()
    {
        var v1 = new Vector([1, 2, 3]);
        var v2 = new Vector([1, 2]);
        Assert.Throws<ArgumentException>(() => _ = v2 + v1);
    }

    [Fact]
    public void VectorEqualsTest()
    {
        var v1 = new Vector([1, 2, 3]);
        var v2 = new Vector([1, 2, 3]);
        Assert.True(v1.Equals(v2));
    }

    [Fact]
    public void VectorEqualityOperatorTest()
    {
        var v1 = new Vector([1, 2, 3]);
        var v2 = new Vector([1, 2, 3]);
        Assert.True(v1 == v2);
    }

    [Fact]
    public void VectorNotEqualsTest()
    {
        var v1 = new Vector([1, 2, 3]);
        var v2 = new Vector([1, 2, 4]);
        Assert.False(v1.Equals(v2));
    }

    [Fact]
    public void VectorInequalityOperatorTest()
    {
        var v1 = new Vector([1, 2, 3]);
        var v2 = new Vector([1, 2, 4]);
        Assert.True(v1 != v2);
    }

    [Fact]
    public void VectorGetHashCodeTest()
    {
        var v1 = new Vector([1, 2]);
        var v2 = new Vector([1, 2]);
        Assert.Equal(v1.GetHashCode(), v2.GetHashCode());
    }
}
