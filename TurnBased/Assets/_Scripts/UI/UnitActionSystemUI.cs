using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform _actionButtonPrefab;
    [SerializeField] private Transform _actionButtonContainerTransform;
    [SerializeField] private TextMeshProUGUI _actionPointsText;

    private List<ActionButtonUI> _actionButtonList;

    private string _actionPointsString = "Action points: ";

    private void Awake()
    {
        _actionButtonList = new List<ActionButtonUI>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;

        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)

    {
        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
    }
    private void UnitActionSystem_OnActionStarted(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void CreateUnitActionButtons()
    {
        foreach (Transform buttonTransform in _actionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject);
        }

        _actionButtonList.Clear();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            Transform actionButtonPrefabTransform = Instantiate(_actionButtonPrefab, _actionButtonContainerTransform);
            ActionButtonUI actionButtonUI = actionButtonPrefabTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseActionText(baseAction);

            _actionButtonList.Add(actionButtonUI);
            Debug.Log(_actionButtonList);
        }
    }

    private void UpdateSelectedVisual()
    {
        foreach (ActionButtonUI actionButtonUI in _actionButtonList)
        {
            actionButtonUI.UpdateSelectedVisual();
        }
    }

    private void UpdateActionPoints()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        _actionPointsText.text = _actionPointsString + selectedUnit.GetActionPoints();    
    }
}
