public class PathNode
{
    private GridPosition _gridPosition;

    public int GCost { get; set; }
    public int HCost { get; set; }
    public int FCost { get; set; }
    
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
}