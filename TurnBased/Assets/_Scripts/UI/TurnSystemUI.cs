using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _turnText;
    [SerializeField] private Button _endTurnButton;
    [SerializeField] private GameObject _enemyTurnVisualObject;

    private string _turnString = "Turn ";

    private void Start()
    {
        _endTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        UpdateCurrentTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();

    }

    private void TurnSystem_OnTurnChanged(object sender, System.EventArgs e)
    {
        UpdateCurrentTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    private void UpdateCurrentTurnText()
    {
        _turnText.text = _turnString + TurnSystem.Instance.TurnNumber;
    }

    private void UpdateEnemyTurnVisual()
    {
        _enemyTurnVisualObject.SetActive(!TurnSystem.Instance.IsPlayerTurn);
    }

    private void UpdateEndTurnButtonVisibility()
    {
        _endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn);
    }

}
