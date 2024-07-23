using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    [SerializeField] private Unit _selectedUnit;

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            _selectedUnit.Move(MouseWorld.GetPosition());
        }
    }
}
