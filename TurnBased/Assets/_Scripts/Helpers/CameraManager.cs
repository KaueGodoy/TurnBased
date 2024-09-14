using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject _actionCameraGameObj;

    private void Start()
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionComplete;

        HideActionCamera();
    }

    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                Unit shooterUnit = shootAction.GetUnit();
                Unit targetUnit = shootAction.GetTargetUnit();

                Vector3 cameraCharacterHeight = Vector3.up * 1.5f;
                Vector3 shootDirection = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;

                float shoulderOffsetAmount = .5f;
                Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDirection * shoulderOffsetAmount;

                Vector3 actionCameraPosition =
                    shooterUnit.GetWorldPosition() + cameraCharacterHeight + shoulderOffset + (shootDirection * -1);
                Vector3 actionCameraRotation = targetUnit.GetWorldPosition() + cameraCharacterHeight;

                _actionCameraGameObj.transform.position = actionCameraPosition;
                _actionCameraGameObj.transform.LookAt(actionCameraRotation);

                ShowActionCamera();
                break;
        }
    }

    private void BaseAction_OnAnyActionComplete(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                HideActionCamera();
                break;
        }
    }

    private void ShowActionCamera()
    {
        _actionCameraGameObj.SetActive(true);
    }

    private void HideActionCamera()
    {
        _actionCameraGameObj.SetActive(false);
    }
}
