using System;
using System.Collections.Generic;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using StarWars.Lib;
using Xunit;

namespace StarWars.Test;

public class ShootCommandTest
{
    public ShootCommandTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();
    }

    private static (Mock<IShootable>, Mock<IGameObjectRepository>, Mock<ICommandReceiver>, Mock<Hwdtech.ICommand>) SetupMocks()
    {
        return (new Mock<IShootable>(), new Mock<IGameObjectRepository>(), new Mock<ICommandReceiver>(), new Mock<Hwdtech.ICommand>());
    }

    [Fact]
    public void Execute_AddsTorpedoToRepository()
    {
        var (mockShootable, mockRepo, mockQueue, mockStartCmd) = SetupMocks();
        mockShootable.Setup(x => x.GetPosition()).Returns(new Vector([10, 20]));
        mockShootable.Setup(x => x.GetVelocity()).Returns(new Vector([1, 0]));

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Actions.Start",
            (Func<object[], object>)(args => mockStartCmd.Object)).Execute();

        new ShootCommand(mockShootable.Object, mockRepo.Object, mockQueue.Object).Execute();

        mockRepo.Verify(x => x.Add(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()), Times.Once);
    }

    [Fact]
    public void Execute_StartsMovingTorpedo()
    {
        var (mockShootable, mockRepo, mockQueue, mockStartCmd) = SetupMocks();
        mockShootable.Setup(x => x.GetPosition()).Returns(new Vector([10, 20]));
        mockShootable.Setup(x => x.GetVelocity()).Returns(new Vector([1, 0]));

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Actions.Start",
            (Func<object[], object>)(args => mockStartCmd.Object)).Execute();

        new ShootCommand(mockShootable.Object, mockRepo.Object, mockQueue.Object).Execute();

        mockStartCmd.Verify(x => x.Execute(), Times.Once);
    }

    [Fact]
    public void Execute_TorpedoHasSpacecraftPositionAndVelocity()
    {
        var position = new Vector([5, 7]);
        var velocity = new Vector([2, -1]);

        var (mockShootable, mockRepo, mockQueue, mockStartCmd) = SetupMocks();
        mockShootable.Setup(x => x.GetPosition()).Returns(position);
        mockShootable.Setup(x => x.GetVelocity()).Returns(velocity);

        IDictionary<string, object> capturedTorpedo = null;
        mockRepo.Setup(x => x.Add(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
            .Callback<string, IDictionary<string, object>>((_, obj) => capturedTorpedo = obj);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Actions.Start",
            (Func<object[], object>)(args => mockStartCmd.Object)).Execute();

        new ShootCommand(mockShootable.Object, mockRepo.Object, mockQueue.Object).Execute();

        Assert.NotNull(capturedTorpedo);
        Assert.Equal(position, capturedTorpedo["position"]);
        Assert.Equal(velocity, capturedTorpedo["velocity"]);
    }

    [Fact]
    public void Execute_GetPositionThrows_Propagates()
    {
        var (mockShootable, mockRepo, mockQueue, _) = SetupMocks();
        mockShootable.Setup(x => x.GetPosition()).Throws<InvalidOperationException>();

        Assert.Throws<InvalidOperationException>(
            () => new ShootCommand(mockShootable.Object, mockRepo.Object, mockQueue.Object).Execute());
    }

    [Fact]
    public void Execute_GetVelocityThrows_Propagates()
    {
        var (mockShootable, mockRepo, mockQueue, _) = SetupMocks();
        mockShootable.Setup(x => x.GetPosition()).Returns(new Vector([0, 0]));
        mockShootable.Setup(x => x.GetVelocity()).Throws<InvalidOperationException>();

        Assert.Throws<InvalidOperationException>(
            () => new ShootCommand(mockShootable.Object, mockRepo.Object, mockQueue.Object).Execute());
    }
}
