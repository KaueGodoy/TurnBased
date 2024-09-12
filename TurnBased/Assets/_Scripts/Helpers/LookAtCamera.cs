using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private Mode _mode;
    //[SerializeField] private bool _invert;

    private Transform _cameraTransform;

    private enum Mode
    {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted,
    }


    private void Awake()
    {
        _cameraTransform = Camera.main.transform;
    }

    //private void LateUpdate()
    //{

    //    if (_invert)
    //    {
    //        Vector3 directionToCamera = (_cameraTransform.position - transform.position).normalized;
    //        transform.LookAt(transform.position + directionToCamera * -1);
    //    }
    //    else
    //    {
    //        transform.LookAt(_cameraTransform);
    //    }
    //}

    private void LateUpdate()
    {
        switch (_mode)
        {
            case Mode.LookAt:
                transform.LookAt(_cameraTransform);
                break;
            case Mode.LookAtInverted:
                Vector3 dirFromCamera = transform.position - _cameraTransform.position;
                transform.LookAt(transform.position + dirFromCamera);
                break;
            case Mode.CameraForward:
                transform.forward = _cameraTransform.forward;
                break;
            case Mode.CameraForwardInverted:
                transform.forward = -_cameraTransform.forward;
                break;
        }
    }

}
