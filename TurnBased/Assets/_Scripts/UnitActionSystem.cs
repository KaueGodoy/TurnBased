using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    [SerializeField] private Unit _selectedUnit;
    [SerializeField] private LayerMask _unitLayerMask;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (TryToHandleUnitSelection()) return;
            _selectedUnit.Move(MouseWorld.GetPosition());
        }

        if (Input.GetMouseButton(1))
        {
            TryToHandleUnitSelection();
        }
    }

    private bool TryToHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _unitLayerMask))
        {
            if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
            {
                _selectedUnit = unit;
                return true;
            }
        }
        return false;
    }
}
