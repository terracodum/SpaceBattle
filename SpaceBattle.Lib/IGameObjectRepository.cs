namespace StarWars.Lib;

public interface IGameObjectRepository
{
    IDictionary<string, object> GetById(string id);
    void Add(string id, IDictionary<string, object> gameObject);
    bool Contains(string id);
}
