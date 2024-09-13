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
