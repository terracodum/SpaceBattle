using Hwdtech;

namespace StarWars.Lib;

public class ShootCommand : ICommand
{
    private readonly IShootable _spacecraft;
    private readonly IGameObjectRepository _repository;
    private readonly ICommandReceiver _queue;

    public ShootCommand(IShootable spacecraft, IGameObjectRepository repository, ICommandReceiver queue)
    {
        _spacecraft = spacecraft;
        _repository = repository;
        _queue = queue;
    }

    public void Execute()
    {
        var torpedoId = Guid.NewGuid().ToString();
        var torpedo = new Dictionary<string, object>
        {
            ["position"] = _spacecraft.GetPosition(),
            ["velocity"] = _spacecraft.GetVelocity()
        };

        _repository.Add(torpedoId, torpedo);

        IoC.Resolve<Hwdtech.ICommand>("Actions.Start", new Dictionary<string, object>
        {
            ["gameObject"] = torpedo,
            ["queue"] = _queue,
            ["cmdType"] = "Move"
        }).Execute();
    }
}
