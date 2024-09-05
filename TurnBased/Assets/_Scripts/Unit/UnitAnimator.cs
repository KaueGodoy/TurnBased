using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private string _isWalking = "IsWalking";
    private string _shoot = "Shoot";

    private void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;
        }
    }

    private void ShootAction_OnShoot(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(_shoot);
    }

    private void MoveAction_OnStopMoving(object sender, System.EventArgs e)
    {
        _animator.SetBool(_isWalking, false);
    }

    private void MoveAction_OnStartMoving(object sender, System.EventArgs e)
    {
        _animator.SetBool(_isWalking, true);
    }
}
