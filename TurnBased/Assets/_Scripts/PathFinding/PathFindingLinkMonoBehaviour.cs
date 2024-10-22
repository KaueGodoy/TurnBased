using UnityEngine;

public class PathFindingLinkMonoBehaviour : MonoBehaviour
{
    [SerializeField] private Vector3 _linkPositionA;
    [SerializeField] private Vector3 _linkPositionB;

    public Vector3 LinkPositionA { get { return _linkPositionA; } set { _linkPositionA = value; } }
    public Vector3 LinkPositionB { get { return _linkPositionB; } set { _linkPositionB = value; } }

    public PathFindingLink GetPathFindingLink()
    {
        return new PathFindingLink
        {
            GridPositionA = LevelGrid.Instance.GetGridPosition(LinkPositionA),
            GridPositionB = LevelGrid.Instance.GetGridPosition(LinkPositionB),
        };
    }
}
