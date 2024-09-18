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
                    if (TryTakeEnemyAIAction(SetStateTakingTurn))
                    {
                        _state = State.Busy;
                    }
                    else
                    {
                        TurnSystem.Instance.NextTurn();
                    }
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

    private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
    {
        Debug.Log("Taking action");

        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            if (TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
            {
                return true;
            }
        }

        return false;
    }

    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
    {
        SpinAction spinAction = enemyUnit.GetSpinAction();

        GridPosition actionGridPosition = enemyUnit.GetGridPosition();

        if (!spinAction.IsValidActionGridPosition(actionGridPosition))
        {
            return false;
        }

        if (!enemyUnit.TrySpendActionPointsToTakeAction(spinAction))
        {
            return false;
        }

        Debug.Log("Spin action");
        spinAction.TakeAction(actionGridPosition, onEnemyAIActionComplete);
        return true;
    }
}
