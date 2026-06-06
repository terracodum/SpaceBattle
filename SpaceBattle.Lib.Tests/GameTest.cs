using System.Collections.Generic;
using Moq;
using StarWars.Lib;
using Xunit;

namespace StarWars.Test;

public class GameTest
{
    private readonly Game _game = new();

    [Fact]
    public void Update_ExecutesCommandFromQueue()
    {
        var mockCmd = new Mock<Hwdtech.ICommand>();
        _game.Receive(mockCmd.Object);

        _game.Update();

        mockCmd.Verify(x => x.Execute(), Times.Once);
    }

    [Fact]
    public void Update_EmptyQueue_DoesNotThrow()
    {
        _game.Update();
    }

    [Fact]
    public void Update_MultipleCommands_ExecutesOnePerCall()
    {
        var mockCmd1 = new Mock<Hwdtech.ICommand>();
        var mockCmd2 = new Mock<Hwdtech.ICommand>();

        _game.Receive(mockCmd1.Object);
        _game.Receive(mockCmd2.Object);

        _game.Update();

        mockCmd1.Verify(x => x.Execute(), Times.Once);
        mockCmd2.Verify(x => x.Execute(), Times.Never);
    }

    [Fact]
    public void Update_ExecutesCommandsInFifoOrder()
    {
        var executionOrder = new List<int>();
        var mockCmd1 = new Mock<Hwdtech.ICommand>();
        var mockCmd2 = new Mock<Hwdtech.ICommand>();

        mockCmd1.Setup(x => x.Execute()).Callback(() => executionOrder.Add(1));
        mockCmd2.Setup(x => x.Execute()).Callback(() => executionOrder.Add(2));

        _game.Receive(mockCmd1.Object);
        _game.Receive(mockCmd2.Object);

        _game.Update();
        _game.Update();

        Assert.Equal([1, 2], executionOrder);
    }


}
