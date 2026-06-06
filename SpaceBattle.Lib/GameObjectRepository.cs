namespace StarWars.Lib;

public class GameObjectRepository : IGameObjectRepository
{
    private readonly Dictionary<string, IDictionary<string, object>> _objects = new();

    public IDictionary<string, object> GetById(string id) => _objects[id];

    public void Add(string id, IDictionary<string, object> gameObject) => _objects[id] = gameObject;

    public bool Contains(string id) => _objects.ContainsKey(id);
}
