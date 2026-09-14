using System.Text.Json;

namespace StarWars.Lib;

public class SaveCollisionDataCommand : ICommand
{
    private readonly ICollisionMap _map;
    private readonly string _filePath;

    public SaveCollisionDataCommand(ICollisionMap map, string filePath)
    {
        _map = map;
        _filePath = filePath;
    }

    public void Execute()
    {
        var json = JsonSerializer.Serialize(_map.GetEntries());
        File.WriteAllText(_filePath, json);
    }
}
