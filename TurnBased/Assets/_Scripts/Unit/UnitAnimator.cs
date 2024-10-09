using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private Transform _bulletProjectilePrefab;
    [SerializeField] private Transform _shootPointTransform;
    [SerializeField] private Transform _rifleTransform;
    [SerializeField] private Transform _swordTransform;


    private string _isWalking = "IsWalking";
    private string _shoot = "Shoot";
    private string _swordSlash = "SwordSlash";

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
        if (TryGetComponent<SwordAction>(out SwordAction swordAction))
        {
            swordAction.OnSwordActionCompleted += SwordAction_OnSwordActionCompleted;
            swordAction.OnSwordActionStarted += SwordAction_OnSwordActionStarted;
        }
    }

    private void Start()
    {
        EquipRifle();
    }

    private void SwordAction_OnSwordActionStarted(object sender, System.EventArgs e)
    {
        EquipSword();
        _animator.SetTrigger(_swordSlash);
    }

    private void SwordAction_OnSwordActionCompleted(object sender, System.EventArgs e)
    {
        EquipRifle();
    }

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        _animator.SetTrigger(_shoot);

        Transform bulletProjectileTransform = Instantiate(_bulletProjectilePrefab, _shootPointTransform.position, Quaternion.identity);

        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e.TargetUnit.GetWorldPosition();
        targetUnitShootAtPosition.y = _shootPointTransform.position.y;


        bulletProjectile.Setup(targetUnitShootAtPosition);
    }

    private void MoveAction_OnStopMoving(object sender, System.EventArgs e)
    {
        _animator.SetBool(_isWalking, false);
    }

    private void MoveAction_OnStartMoving(object sender, System.EventArgs e)
    {
        _animator.SetBool(_isWalking, true);
    }

    private void EquipSword()
    {
        _swordTransform.gameObject.SetActive(true);
        _rifleTransform.gameObject.SetActive(false);
    }

    private void EquipRifle()
    {
        _swordTransform.gameObject.SetActive(false);
        _rifleTransform.gameObject.SetActive(true);
    }
}
