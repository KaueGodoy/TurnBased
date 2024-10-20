using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;

    [SerializeField] private Unit _selectedUnit;
    [SerializeField] private LayerMask _unitLayerMask;

    private BaseAction _selectedAction;
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

    private void Start()
    {
        SetSelectedUnit(_selectedUnit);
    }

    private void Update()
    {
        Debug.Log(LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition()));

        if (_isBusy) return;

        if (!TurnSystem.Instance.IsPlayerTurn) return;

        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (TryToHandleUnitSelection()) return;

        HandleSelectedAction();
    }

    private void HandleSelectedAction()
    {
        if (InputManager.Instance.IsMouseButtonDownThisFrame())
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if (!_selectedAction.IsValidActionGridPosition(mouseGridPosition)) return;
            if (!_selectedUnit.TrySpendActionPointsToTakeAction(_selectedAction)) return;

            SetBusy();
            _selectedAction.TakeAction(mouseGridPosition, ClearBusy);
            OnActionStarted?.Invoke(this, EventArgs.Empty);

            //switch (_selectedAction)
            //{
            //    case MoveAction moveAction:
            //        if (moveAction.IsValidActionGridPosition(mouseGridPosition))
            //        {
            //            SetBusy();
            //            moveAction.Move(mouseGridPosition, ClearBusy);
            //        }
            //        break;
            //    case SpinAction spinAction:
            //        SetBusy();
            //        spinAction.Spin(ClearBusy);
            //        break;
            //}
        }
    }

    private void SetBusy()
    {
        _isBusy = true;
        OnBusyChanged?.Invoke(this, _isBusy);
    }

    private void ClearBusy()
    {
        _isBusy = false;
        OnBusyChanged?.Invoke(this, _isBusy);
    }

    private bool TryToHandleUnitSelection()
    {
        if (InputManager.Instance.IsMouseButtonDownThisFrame())
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _unitLayerMask))
            {
                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    if (unit == _selectedUnit)
                    {
                        return false;
                    }

                    if (unit.IsEnemy)
                    {
                        return false;
                    }

                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }
        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        _selectedUnit = unit;
        SetSelectedAction(unit.GetAction<MoveAction>());

        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        _selectedAction = baseAction;

        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return _selectedUnit;
    }

    public BaseAction GetSelectedAction()
    {
        return _selectedAction;
    }
}
