using UnityEngine;

public class Testing : MonoBehaviour
{
    private GridSystem _gridSystem;

    private void Start()
    {
        _gridSystem = new GridSystem(10, 10, 2f);

        Debug.Log(new GridPosition(2, 4));
    }

    private void Update()
    {
        Debug.Log(_gridSystem.GetGridPosition(MouseWorld.GetPosition()));
    }
}
