using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }

    [SerializeField] private Transform _gridDebugPrefab;

    private GridSystem _gridSystem;

    private void Awake()
    {

        if (Instance != null)
        {
            Debug.LogError("LevelGrid Instance already exists" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _gridSystem = new GridSystem(10, 10, 2f);
        _gridSystem.CreateDebugObjects(_gridDebugPrefab);
    }

    public void SetUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        gridObject.SetUnit(unit);
    }

    public void GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        gridObject.GetUnit();
    }

    public void ClearUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        gridObject.SetUnit(null);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) => _gridSystem.GetGridPosition(worldPosition);

}
