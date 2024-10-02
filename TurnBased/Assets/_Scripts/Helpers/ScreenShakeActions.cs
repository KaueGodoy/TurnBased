using UnityEngine;

public class ScreenShakeActions : MonoBehaviour
{
    [SerializeField] private float _shakeAmount = 2f;

    private void Start()
    {
        ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
    }

    private void ShootAction_OnAnyShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        ScreenShake.Instance.Shake(_shakeAmount);
    }
}
