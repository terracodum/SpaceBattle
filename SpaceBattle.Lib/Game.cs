namespace StarWars.Lib;

public class Game : ICommandReceiver
{
    private readonly Queue<Hwdtech.ICommand> _commandQueue = new();

    public void Receive(Hwdtech.ICommand cmd) => _commandQueue.Enqueue(cmd);

    public void Update()
    {
        if (_commandQueue.Count > 0)
        {
            _commandQueue.Dequeue().Execute();
        }
    }
}
