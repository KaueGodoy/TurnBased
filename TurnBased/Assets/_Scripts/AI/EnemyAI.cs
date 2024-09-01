using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float _timer;

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void TurnSystem_OnTurnChanged(object sender, System.EventArgs e)
    {
        _timer = 2f;
    }

    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn) return;

        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            TurnSystem.Instance.NextTurn();
        }
    }
}
