using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Transform _gridDebugPrefab;

    private GridSystem _gridSystem;

    private void Start()
    {
        _gridSystem = new GridSystem(10, 10, 2f);
        _gridSystem.CreateDebugObjects(_gridDebugPrefab);

        Debug.Log(new GridPosition(2, 4));
    }

    private void Update()
    {
        Debug.Log(_gridSystem.GetGridPosition(MouseWorld.GetPosition()));
    }
}
