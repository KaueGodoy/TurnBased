public class PathNode
{
    private GridPosition _gridPosition;

    public int GCost { get; set; }
    public int HCost { get; set; }
    public int FCost { get; set; }

    private bool _isWalkable = true;
    public bool IsWalkable { get { return _isWalkable; } set { _isWalkable = value; } }


    private PathNode _cameFromPathNode;

    public PathNode(GridPosition gridPosition)
    {
        _gridPosition = gridPosition;
    }

    public override string ToString()
    {
        return _gridPosition.ToString();
    }

    public void CalculateFCost()
    {
        FCost = GCost + HCost;
    }

    public void ResetCameFromPathNode()
    {
        _cameFromPathNode = null;
    }

    public void SetCameFromPathNode(PathNode pathNode)
    {
        _cameFromPathNode = pathNode;
    }

    public PathNode GetCameFromPathNode()
    {
        return _cameFromPathNode;
    }

    public GridPosition GetGridPosition()
    {
        return _gridPosition;
    }
}