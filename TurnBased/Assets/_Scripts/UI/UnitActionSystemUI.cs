using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform _actionButtonPrefab;
    [SerializeField] private Transform _actionButtonContainerTransform;

    private List<ActionButtonUI> _actionButtonList;

    private void Awake()
    {
        _actionButtonList = new List<ActionButtonUI>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;

        CreateUnitActionButtons();
        UpdateSelectedVisual();
    }


    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
        UpdateSelectedVisual();
    }
    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
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
}
