namespace StarWars.Lib;

public class ShapeFootprint : IShapeFootprint
{
    public string ShapeId { get; }
    public IReadOnlyList<(int X, int Y)> Cells { get; }

    public ShapeFootprint(string shapeId, IEnumerable<(int X, int Y)> cells)
    {
        ShapeId = shapeId;
        Cells = cells.ToList();
    }
}
