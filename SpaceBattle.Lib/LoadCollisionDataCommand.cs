using System.Text.Json;

namespace StarWars.Lib;

public class LoadCollisionDataCommand : ICommand
{
    private readonly ICollisionMap _map;
    private readonly string _filePath;

    public LoadCollisionDataCommand(ICollisionMap map, string filePath)
    {
        _map = map;
        _filePath = filePath;
    }

    public void Execute()
    {
        var json = File.ReadAllText(_filePath);
        var entries = JsonSerializer.Deserialize<List<CollisionEntry>>(json)
            ?? throw new InvalidDataException("Collision data file is empty or malformed.");

        foreach (var entry in entries)
        {
            _map.Add(entry.ShapeA, entry.ShapeB, entry.RelX, entry.RelY);
        }
    }
}
