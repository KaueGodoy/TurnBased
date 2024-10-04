using UnityEngine;

public class ScreenShakeActions : MonoBehaviour
{
    [SerializeField] private float _bulletShakeAmount = 2f;
    [SerializeField] private float _grenadeShakeAmount = 5f;

    private void Start()
    {
        ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
        GrenadeProjectile.OnAnyGrenadeExploded += GrenadeProjectile_OnAnyGrenadeExploded;
    }

    private void GrenadeProjectile_OnAnyGrenadeExploded(object sender, System.EventArgs e)
    {
        ScreenShake.Instance.Shake(_grenadeShakeAmount);
    }

    private void ShootAction_OnAnyShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        ScreenShake.Instance.Shake(_bulletShakeAmount);
    }
}
