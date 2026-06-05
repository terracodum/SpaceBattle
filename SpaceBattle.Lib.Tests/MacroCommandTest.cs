using System;
using System.Collections.Generic;
using Moq;
using StarWars.Lib;
using Xunit;

namespace StarWars.Test;

public class MacroCommandTests
{
    [Fact]
    public void MacroCommand_ExecutesAllCommandsFromArray()
    {
        var mock1 = new Mock<Hwdtech.ICommand>();
        var mock2 = new Mock<Hwdtech.ICommand>();
        var mock3 = new Mock<Hwdtech.ICommand>();

        var macro = new MacroCommand(new List<Hwdtech.ICommand> { mock1.Object, mock2.Object, mock3.Object });
        macro.Execute();

        mock1.Verify(c => c.Execute(), Times.Once);
        mock2.Verify(c => c.Execute(), Times.Once);
        mock3.Verify(c => c.Execute(), Times.Once);
    }

    [Fact]
    public void MacroCommand_WhenCommandThrows_PropagatesExceptionAndSkipsRemainingCommands()
    {
        var mock1 = new Mock<Hwdtech.ICommand>();
        var mock2 = new Mock<Hwdtech.ICommand>();
        var mock3 = new Mock<Hwdtech.ICommand>();

        mock2.Setup(c => c.Execute()).Throws<Exception>();

        var macro = new MacroCommand(new List<Hwdtech.ICommand> { mock1.Object, mock2.Object, mock3.Object });

        Assert.Throws<Exception>(() => macro.Execute());
        mock1.Verify(c => c.Execute(), Times.Once);
        mock3.Verify(c => c.Execute(), Times.Never);
    }
}
