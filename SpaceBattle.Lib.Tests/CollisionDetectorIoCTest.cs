using System;
using System.Collections.Generic;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using StarWars.Lib;
using Xunit;

namespace StarWars.Test;

public class CollisionDetectorIoCTest
{
    public CollisionDetectorIoCTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();
    }

    [Fact]
    public void Execute_RegistersDetectorsCollision_ReturnsCollisionDetector()
    {
        new RegisterIoCDependencyCollisionDetector().Execute();

        var map = new CollisionMap();
        var detector = IoC.Resolve<ICollisionDetector>("Detectors.Collision", map);

        Assert.NotNull(detector);
        Assert.IsType<CollisionDetector>(detector);
    }

    [Fact]
    public void Execute_RegistersCommandsCheckCollision_ReturnsCheckCollisionCommand()
    {
        var mockCollidable = new Mock<ICollidable>();
        var mockRepo = new Mock<ICollidableRepository>();
        var mockDetector = new Mock<ICollisionDetector>();
        var mockOnCollision = new Mock<Hwdtech.ICommand>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Adapters.ICollidable",
            (Func<object, object>)(obj => mockCollidable.Object)).Execute();

        new RegisterIoCDependencyCollisionDetector().Execute();

        var order = new Dictionary<string, object>
        {
            ["selfId"] = "ship-1",
            ["gameObject"] = new Dictionary<string, object>(),
            ["repository"] = mockRepo.Object,
            ["detector"] = mockDetector.Object,
            ["onCollision"] = mockOnCollision.Object
        };

        var cmd = IoC.Resolve<Hwdtech.ICommand>("Commands.CheckCollision", order);

        Assert.NotNull(cmd);
        Assert.IsType<CheckCollisionCommand>(cmd);
    }
}
