using System;
using System.Collections.Generic;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using StarWars.Lib;
using Xunit;

namespace StarWars.Test;

public class CreateMacroCommandStrategyTests
{
    public CreateMacroCommandStrategyTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        var iocScope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void Resolve_WithValidDependencies_ReturnsExecutableMacroCommand()
    {
        var mockCmd1 = new Mock<Hwdtech.ICommand>();
        var mockCmd2 = new Mock<Hwdtech.ICommand>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Specs.Test",
            (Func<object[], object>)(a => new List<string> { "Commands.Test1", "Commands.Test2" })).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.Test1",
            (Func<object[], object>)(a => mockCmd1.Object)).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.Test2",
            (Func<object[], object>)(a => mockCmd2.Object)).Execute();

        var strategy = new CreateMacroCommandStrategy("Test");
        var macroCommand = strategy.Resolve(Array.Empty<object>());

        macroCommand.Execute();

        mockCmd1.Verify(c => c.Execute(), Times.Once);
        mockCmd2.Verify(c => c.Execute(), Times.Once);
    }

    [Fact]
    public void Resolve_WhenSpecsDependencyMissing_ThrowsException()
    {
        var strategy = new CreateMacroCommandStrategy("NotRegistered");

        Assert.Throws<ArgumentException>(() => strategy.Resolve(Array.Empty<object>()));
    }

    [Fact]
    public void Resolve_WhenCommandDependencyMissing_ThrowsException()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Specs.TestMissingCmd",
            (Func<object[], object>)(a => new List<string> { "Commands.NonExistent" })).Execute();

        var strategy = new CreateMacroCommandStrategy("TestMissingCmd");

        Assert.Throws<ArgumentException>(() => strategy.Resolve(Array.Empty<object>()));
    }
}
