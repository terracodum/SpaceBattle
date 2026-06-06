using StarWars.Lib;
using Xunit;

namespace StarWars.Test;

public class CollisionMapTest
{
    private readonly CollisionMap _map = new();

    [Fact]
    public void Contains_AfterAdd_ReturnsTrue()
    {
        _map.Add("ship", "torpedo", 0, 0);

        Assert.True(_map.Contains("ship", "torpedo", 0, 0));
    }

    [Fact]
    public void Contains_NotAdded_ReturnsFalse()
    {
        Assert.False(_map.Contains("ship", "torpedo", 5, 5));
    }

    [Fact]
    public void Contains_Symmetric_ReturnsTrue()
    {
        _map.Add("ship", "torpedo", 3, -2);

        Assert.True(_map.Contains("torpedo", "ship", -3, 2));
    }

    [Fact]
    public void GetEntries_ReturnsAllAddedEntries()
    {
        _map.Add("A", "B", 0, 0);
        _map.Add("A", "B", 1, 0);
        _map.Add("A", "C", 0, 1);

        Assert.Equal(3, _map.GetEntries().Count);
    }

    [Fact]
    public void Add_DuplicateEntry_DoesNotDuplicate()
    {
        _map.Add("A", "B", 0, 0);
        _map.Add("A", "B", 0, 0);

        Assert.Single(_map.GetEntries());
    }
}
