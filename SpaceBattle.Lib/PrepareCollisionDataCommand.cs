namespace StarWars.Lib;

// For each pair of occupied cells across the two shapes, computes the relative
// position of B's centre from A's centre that causes those cells to overlap.
// Result is stored in the ICollisionMap so that collision checks are O(1).
public class PrepareCollisionDataCommand : ICommand
{
    private readonly IShapeFootprint _shapeA;
    private readonly IShapeFootprint _shapeB;
    private readonly ICollisionMap _map;

    public PrepareCollisionDataCommand(IShapeFootprint shapeA, IShapeFootprint shapeB, ICollisionMap map)
    {
        _shapeA = shapeA;
        _shapeB = shapeB;
        _map = map;
    }

    public void Execute()
    {
        foreach (var cellA in _shapeA.Cells)
        {
            foreach (var cellB in _shapeB.Cells)
            {
                _map.Add(_shapeA.ShapeId, _shapeB.ShapeId, cellA.X - cellB.X, cellA.Y - cellB.Y);
            }
        }
    }
}
