using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private Button _actionButton;
    [SerializeField] private TextMeshProUGUI _actionText;

    public void SetBaseActionText(BaseAction baseAction)
    {
        _actionText.text = baseAction.GetActionName().ToUpper();

        _actionButton.onClick.AddListener(() =>
        {
            UnitActionSystem.Instance.SetSelectedAction(baseAction);    
        });
    }

}
