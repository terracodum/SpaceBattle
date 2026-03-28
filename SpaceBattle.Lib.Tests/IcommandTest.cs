using StarWars.Lib; // добавьте это
using Xunit;

namespace SpaceBattle.Test;

public class ICommandTest
{
    [Fact]
    public void InterfaceIsAvailable()
    {
        Assert.NotNull(typeof(ICommand));
    }
}
