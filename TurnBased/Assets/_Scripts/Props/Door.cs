using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private bool _isOpen;

    private GridPosition _gridPosition;
    private Animator _animator;

    private bool _isActive;
    private float _timer;
    private string _isOpenAnimation = "IsOpen";

    private Action _onInteractionComplete;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }


    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetDoorAtGridPosition(_gridPosition, this);

        if (_isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    private void Update()
    {
        if (!_isActive) return;


        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            _isActive = false;
            _onInteractionComplete();
        }

    }

    public void Interact(Action onInteractionComplete)
    {
        this._onInteractionComplete = onInteractionComplete;
        _isActive = true;
        _timer = .5f;

        Debug.Log("Interact");

        if (_isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        _isOpen = true;
        _animator.SetBool(_isOpenAnimation, _isOpen);
        PathFinding.Instance.SetWalkableGridPosition(_gridPosition, true);
    }

    private void CloseDoor()
    {
        _isOpen = false;
        _animator.SetBool(_isOpenAnimation, _isOpen);
        PathFinding.Instance.SetWalkableGridPosition(_gridPosition, false);
    }
}
