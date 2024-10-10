using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private bool _isOpen;

    private GridPosition _gridPosition;

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

    public void Interact()
    {
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
        PathFinding.Instance.SetWalkableGridPosition(_gridPosition, true);
    }

    private void CloseDoor()
    {
        _isOpen = false;
        PathFinding.Instance.SetWalkableGridPosition(_gridPosition, false);
    }
}
