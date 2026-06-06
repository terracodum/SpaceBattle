namespace StarWars.Lib;

public class CheckAuthorizationCommand : Hwdtech.ICommand
{
    private readonly IAuthorizationService _authService;
    private readonly string _gameId;
    private readonly string _objectId;
    private readonly string _playerId;

    public CheckAuthorizationCommand(IAuthorizationService authService, string gameId, string objectId, string playerId)
    {
        _authService = authService;
        _gameId = gameId;
        _objectId = objectId;
        _playerId = playerId;
    }

    public void Execute()
    {
        if (!_authService.IsAuthorized(_gameId, _objectId, _playerId))
        {
            throw new UnauthorizedAccessException(
                $"Player '{_playerId}' is not authorized to act on object '{_objectId}' in game '{_gameId}'.");
        }
    }
}
