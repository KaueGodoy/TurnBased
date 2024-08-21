using System;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Unit _unit;
    protected bool _isActive;
    protected Action _onActionComplete;

    protected string _actionName = "Default name";
    public string ActionName { get { return _actionName; } set { _actionName = value; } }
    public abstract string GetActionName();

    protected virtual void Awake()
    {
        _unit = GetComponent<Unit>();
    }
}
