using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }

    [SerializeField] private List<Unit> _unitList;
    [SerializeField] private List<Unit> _friendlyUnitList;
    [SerializeField] private List<Unit> _enemyUnitList;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Instance already exists" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _unitList = new List<Unit>();
        _friendlyUnitList = new List<Unit>();
        _enemyUnitList = new List<Unit>();
    }

    private void Start()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    private void Unit_OnAnyUnitDead(object sender, System.EventArgs e)
    {

        Unit unit = sender as Unit;
        Debug.Log("Dead");

        _unitList.Remove(unit);

        if (unit.IsEnemy)
        {
            _enemyUnitList.Remove(unit);
        }
        else
        {
            _friendlyUnitList.Remove(unit);
        }
    }

    private void Unit_OnAnyUnitSpawned(object sender, System.EventArgs e)
    {

        Unit unit = sender as Unit;

        Debug.Log("Spawned");
        _unitList.Add(unit);

        if (unit.IsEnemy)
        {
            _enemyUnitList.Add(unit);
        }
        else
        {
            _friendlyUnitList.Add(unit);    
        }
    }

    public List<Unit> GetUnitList()
    {
        return _unitList;
    }

    public List<Unit> GetFriendlyUnitList()
    {
        return _friendlyUnitList;
    }

    public List<Unit> GetEnemyUnitList()
    {
        return _enemyUnitList;
    }
}
