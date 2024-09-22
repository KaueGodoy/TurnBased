public class PathNode
{
    private GridPosition _gridPosition;

    private int _gCost;
    private int _hCost;
    private int _fCost;
    
    private PathNode _cameFromPathNode;

    public PathNode(GridPosition gridPosition)
    {
        _gridPosition = gridPosition;
    }

    public override string ToString()
    {
        return _gridPosition.ToString();
    }
}