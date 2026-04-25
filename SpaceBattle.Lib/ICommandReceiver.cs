namespace StarWars.Lib;

public interface ICommandReceiver
{
    void Receive(Hwdtech.ICommand cmd);
}
