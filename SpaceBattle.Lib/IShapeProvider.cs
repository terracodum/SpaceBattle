namespace StarWars.Lib;

public interface IShapeProvider
{
    IShapeFootprint GetShape(string shapeId);
}
