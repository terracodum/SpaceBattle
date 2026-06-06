namespace StarWars.Lib;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public class AdapterAttribute : Attribute
{
    public Type InterfaceType { get; }
    public string PropertyName { get; }
    public Type ResolverType { get; }

    public AdapterAttribute(Type interfaceType, string propertyName, Type resolverType)
    {
        InterfaceType = interfaceType;
        PropertyName = propertyName;
        ResolverType = resolverType;
    }
}
