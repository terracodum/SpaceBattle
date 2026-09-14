using System.Collections.Generic;
using System.Reflection;
using Hwdtech;
using Hwdtech.Ioc;
using StarWars.Lib;
using Xunit;

[assembly: StarWars.Lib.Adapter(
    typeof(StarWars.Tests.IMovingObject),
    "Velocity",
    typeof(StarWars.Tests.TestVelocityResolver))]

namespace StarWars.Tests;

public static class TestVelocityResolver
{
    public static object Resolve(IDictionary<string, object> dict) => dict["vel"];
}

public class RegisterAdapterDependencyTests
{
    private readonly IReadOnlyList<IMethodHandler> _handlers =
        [new GetterMethodHandler(), new SetterMethodHandler()];

    public RegisterAdapterDependencyTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();
        new InitScopeBasedIoCImplementationCommand().Execute();
    }

    private RegisterAdapterDependency<IMovingObject> BuildCommand()
    {
        var strategies = new AdapterStrategyReader<IMovingObject>(Assembly.GetExecutingAssembly()).Read();
        var factory = new DictionaryAdapterFactory<IMovingObject>(_handlers);
        return new RegisterAdapterDependency<IMovingObject>(factory, strategies);
    }

    [Fact]
    public void Execute_RegistersAdapterInIoC()
    {
        BuildCommand().Execute();

        var dict = new Dictionary<string, object>
        {
            ["Location"] = new Vector([1, 2]),
            ["vel"] = new Vector([3, 4])
        };

        var adapter = IoC.Resolve<IMovingObject>("Adapters.IMovingObject", dict);

        Assert.NotNull(adapter);
        Assert.IsAssignableFrom<IMovingObject>(adapter);
    }

    [Fact]
    public void Execute_AdapterReadsLocationFromDictionary()
    {
        BuildCommand().Execute();

        var location = new Vector([7, 8]);
        var dict = new Dictionary<string, object> { ["Location"] = location, ["vel"] = new Vector([0, 0]) };

        var adapter = IoC.Resolve<IMovingObject>("Adapters.IMovingObject", dict);

        Assert.Equal(location, adapter.Location);
    }

    [Fact]
    public void Execute_AdapterUsesCustomStrategyForVelocity()
    {
        BuildCommand().Execute();

        var velocity = new Vector([5, 6]);
        var dict = new Dictionary<string, object> { ["Location"] = new Vector([0, 0]), ["vel"] = velocity };

        var adapter = IoC.Resolve<IMovingObject>("Adapters.IMovingObject", dict);

        Assert.Equal(velocity, adapter.Velocity);
    }
}
