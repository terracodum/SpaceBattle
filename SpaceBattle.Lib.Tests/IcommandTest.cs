using Xunit;
using StarWars.Lib; // добавьте это

namespace SpaceBattle.Test;

public class ICommandTest
{
    [Fact]
    public void InterfaceIsAvailable()
    {
        Assert.NotNull(typeof(ICommand));
    }
}