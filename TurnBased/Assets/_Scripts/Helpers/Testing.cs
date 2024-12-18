using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Unit _unit;

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
           
        }
    }

    private void TestPathFindingFromStartToMousePosition()
    {
        GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPositionOnlyHitVisible());
        GridPosition startGridPosition = new GridPosition(0, 0, 0);

        List<GridPosition> gridPositionList = PathFinding.Instance.FindPath(startGridPosition, mouseGridPosition, out int pathLength);

        for (int i = 0; i < gridPositionList.Count - 1; i++)
        {
            Debug.DrawLine(
                LevelGrid.Instance.GetWorldPosition(gridPositionList[i]),
                LevelGrid.Instance.GetWorldPosition(gridPositionList[i + 1]),
                Color.green,
                10f
                );
        }
    }

    private void TestShakeScreen(float intensity)
    {
        ScreenShake.Instance.Shake(intensity);
    }
}