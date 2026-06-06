using System.Collections.Generic;
using StarWars.Lib;
using Xunit;

namespace StarWars.Test;

public class GameObjectRepositoryTest
{
    private readonly GameObjectRepository _repository = new();

    [Fact]
    public void Add_ThenGetById_ReturnsAddedObject()
    {
        var obj = new Dictionary<string, object> { ["x"] = 42 };

        _repository.Add("obj1", obj);

        Assert.Equal(obj, _repository.GetById("obj1"));
    }

    [Fact]
    public void GetById_NotFound_ThrowsKeyNotFoundException()
    {
        Assert.Throws<KeyNotFoundException>(() => _repository.GetById("nonexistent"));
    }

    [Fact]
    public void Contains_ExistingId_ReturnsTrue()
    {
        _repository.Add("obj1", new Dictionary<string, object>());

        Assert.True(_repository.Contains("obj1"));
    }

    [Fact]
    public void Contains_NonExistingId_ReturnsFalse()
    {
        Assert.False(_repository.Contains("nonexistent"));
    }

    [Fact]
    public void Add_OverwritesExistingObject()
    {
        var first = new Dictionary<string, object> { ["v"] = 1 };
        var second = new Dictionary<string, object> { ["v"] = 2 };

        _repository.Add("id", first);
        _repository.Add("id", second);

        Assert.Equal(second, _repository.GetById("id"));
    }
}
