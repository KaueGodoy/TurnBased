using System;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }

    public event EventHandler OnTurnChanged;

    [SerializeField] private int _turnNumber = 1;
    public int TurnNumber {  get { return _turnNumber; } set { _turnNumber = value; } }   

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Instance already exists" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void NextTurn()
    {
        TurnNumber++;
        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetTurnNumber()
    {
        return _turnNumber;
    }
}
