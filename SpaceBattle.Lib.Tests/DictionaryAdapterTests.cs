using System;
using System.Collections.Generic;
using StarWars.Lib;
using Xunit;

namespace StarWars.Tests;

public interface IMovingObject
{
    Vector Location { get; set; }
    Vector Velocity { get; }
}

public static class VelocityResolver
{
    public static object Resolve(IDictionary<string, object> dict) =>
        dict["vel"];
}

public class DictionaryAdapterTests
{
    private static readonly IReadOnlyDictionary<string, Func<IDictionary<string, object>, object>> NoStrategies =
        new Dictionary<string, Func<IDictionary<string, object>, object>>();

    [Fact]
    public void Get_ReadsPropertyFromDictionary()
    {
        var location = new Vector([1, 2]);
        var dict = new Dictionary<string, object> { ["Location"] = location, ["Velocity"] = new Vector([0, 0]) };

        var adapter = DictionaryAdapter<IMovingObject>.Create(dict, NoStrategies);

        Assert.Equal(location, adapter.Location);
    }

    [Fact]
    public void Set_WritesPropertyToDictionary()
    {
        var dict = new Dictionary<string, object> { ["Location"] = new Vector([0, 0]), ["Velocity"] = new Vector([0, 0]) };
        var adapter = DictionaryAdapter<IMovingObject>.Create(dict, NoStrategies);
        var newLocation = new Vector([5, 5]);

        adapter.Location = newLocation;

        Assert.Equal(newLocation, dict["Location"]);
    }

    [Fact]
    public void Get_UsesCustomStrategyWhenProvided()
    {
        var velocity = new Vector([3, 4]);
        var dict = new Dictionary<string, object> { ["Location"] = new Vector([0, 0]), ["vel"] = velocity };
        var strategies = new Dictionary<string, Func<IDictionary<string, object>, object>>
        {
            ["Velocity"] = VelocityResolver.Resolve
        };

        var adapter = DictionaryAdapter<IMovingObject>.Create(dict, strategies);

        Assert.Equal(velocity, adapter.Velocity);
    }

    [Fact]
    public void Get_ThrowsWhenKeyMissingAndNoStrategy()
    {
        var dict = new Dictionary<string, object>();
        var adapter = DictionaryAdapter<IMovingObject>.Create(dict, NoStrategies);

        Assert.Throws<KeyNotFoundException>(() => adapter.Velocity);
    }
}
