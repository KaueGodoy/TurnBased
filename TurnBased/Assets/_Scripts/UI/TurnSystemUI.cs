using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _turnText;
    [SerializeField] private Button _endTurnButton;

    private string _turnString = "Turn ";

    private void Start()
    {
        _endTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        UpdateCurrentTurnText();
    }

    private void TurnSystem_OnTurnChanged(object sender, System.EventArgs e)
    {
        UpdateCurrentTurnText();
    }

    private void UpdateCurrentTurnText()
    {
        _turnText.text = _turnString + TurnSystem.Instance.TurnNumber;
    }

}
