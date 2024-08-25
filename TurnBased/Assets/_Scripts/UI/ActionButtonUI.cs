using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private Button _actionButton;
    [SerializeField] private TextMeshProUGUI _actionText;
    [SerializeField] private GameObject _selectedVisualGameObj;

    private BaseAction _baseAction;

    public void SetBaseActionText(BaseAction baseAction)
    {
        this._baseAction = baseAction;
        _actionText.text = baseAction.GetActionName().ToUpper();

        _actionButton.onClick.AddListener(() =>
        {
            UnitActionSystem.Instance.SetSelectedAction(baseAction);    
        });
    }

    public void UpdateSelectedVisual()
    {
        BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
        _selectedVisualGameObj.SetActive(selectedBaseAction == _baseAction);
    }

}
