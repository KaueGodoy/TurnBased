using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float _timer;

    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn, 
        Busy
    }

    private State _state;

    private void Awake()
    {
        _state = State.WaitingForEnemyTurn;   
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void TurnSystem_OnTurnChanged(object sender, System.EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn)
        {
            _state = State.TakingTurn;
            _timer = 2f;
        }
    }

    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn) return;

        switch (_state)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                _timer -= Time.deltaTime;
                if (_timer <= 0f)
                {
                    _state = State.Busy;
                    TakeEnemyAIAction(SetStateTakingTurn);
                }
                break;
            case State.Busy:
                break;
        }

    }

    private void SetStateTakingTurn()
    {
        _timer = 0.5f;
        _state = State.TakingTurn;
    }

    private void TakeEnemyAIAction(Action onEnemyAIActionComplete)
    {
        Debug.Log("Taking action");

        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            TakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete);
        }
    }

    private void TakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
    {
        SpinAction spinAction = enemyUnit.GetSpinAction();

        GridPosition actionGridPosition = enemyUnit.GetGridPosition();

        if (!spinAction.IsValidActionGridPosition(actionGridPosition))
        {
            return;
        }

        if (!enemyUnit.TrySpendActionPointsToTakeAction(spinAction))
        {
            return;
        }

        Debug.Log("Spin action");
        spinAction.TakeAction(actionGridPosition, onEnemyAIActionComplete);
    }
}
