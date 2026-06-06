using System;
using System.IO;
using StarWars.Lib;
using Xunit;

namespace StarWars.Test;

public class SaveLoadCollisionDataTest : IDisposable
{
    private readonly string _filePath = Path.GetTempFileName();

    [Fact]
    public void SaveThenLoad_RoundTrip_ProducesIdenticalEntries()
    {
        var source = new CollisionMap();
        source.Add("ship", "torpedo", 0, 0);
        source.Add("ship", "torpedo", 1, 0);
        source.Add("asteroid", "ship", -2, 3);

        new SaveCollisionDataCommand(source, _filePath).Execute();

        var target = new CollisionMap();
        new LoadCollisionDataCommand(target, _filePath).Execute();

        foreach (var entry in source.GetEntries())
        {
            Assert.True(target.Contains(entry.ShapeA, entry.ShapeB, entry.RelX, entry.RelY));
        }

        Assert.Equal(source.GetEntries().Count, target.GetEntries().Count);
    }

    [Fact]
    public void Save_CreatesValidJsonFile()
    {
        var map = new CollisionMap();
        map.Add("A", "B", 1, 2);

        new SaveCollisionDataCommand(map, _filePath).Execute();

        var content = File.ReadAllText(_filePath);
        Assert.Contains("ShapeA", content);
        Assert.Contains("ShapeB", content);
        Assert.Contains("RelX", content);
        Assert.Contains("RelY", content);
    }

    [Fact]
    public void Load_EmptyMap_LoadsAllEntries()
    {
        var source = new CollisionMap();
        source.Add("X", "Y", 5, -3);
        new SaveCollisionDataCommand(source, _filePath).Execute();

        var target = new CollisionMap();
        new LoadCollisionDataCommand(target, _filePath).Execute();

        Assert.True(target.Contains("X", "Y", 5, -3));
    }

    public void Dispose()
    {
        if (File.Exists(_filePath))
        {
            File.Delete(_filePath);
        }
    }
}
