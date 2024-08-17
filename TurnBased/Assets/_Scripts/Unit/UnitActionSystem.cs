using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChanged;

    [SerializeField] private Unit _selectedUnit;
    [SerializeField] private LayerMask _unitLayerMask;

    private bool _isBusy = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Instance already exists" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {

        if (_isBusy) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (TryToHandleUnitSelection()) return;

            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if (_selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition))
            {
                SetBusy();
                _selectedUnit.GetMoveAction().Move(mouseGridPosition);
            }
        }

        if (Input.GetMouseButton(1))
        {
            SetBusy();
            _selectedUnit.GetSpinAction().Spin(ClearBusy);
        }
    }

    private void SetBusy()
    {
        _isBusy = true;
    }

    private void ClearBusy()
    {
        _isBusy = false;    
    }

    private bool TryToHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _unitLayerMask))
        {
            if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
            {
                SetSelectedUnit(unit);
                return true;
            }
        }
        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        _selectedUnit = unit;

        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelecteUnit()
    {
        return _selectedUnit;
    }
}
