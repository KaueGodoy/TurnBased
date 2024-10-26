using Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    private const float Min_Follow_Y_Offset = 2f;
    private const float Max_Follow_Y_Offset = 15f;

    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;

    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _rotationSpeed = 50f;
    [SerializeField] private float _zoomAmount = 1f;
    [SerializeField] private float _zoomSpeed = 3f;
    public float MoveSpeed { get { return _moveSpeed; } set { value = _moveSpeed; } }
    public float RotationSpeed { get { return _rotationSpeed; } set { value = _rotationSpeed; } }
    public float ZoomAmount { get { return _zoomAmount; } set { value = _zoomAmount; } }
    public float ZoomSpeed { get { return _zoomSpeed; } set { value = _zoomSpeed; } }

    private CinemachineTransposer _cinemachineTransposer;
    private Vector3 _targetFollowOffset;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _cinemachineTransposer = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        _targetFollowOffset = _cinemachineTransposer.m_FollowOffset;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    private void HandleMovement()
    {
        Vector2 inputMoveDir = InputManager.Instance.GetCameraMoveVector();

        Vector3 moveVector = transform.forward * inputMoveDir.y + transform.right * inputMoveDir.x;
        transform.position += moveVector * MoveSpeed * Time.deltaTime;
    }

    private void HandleRotation()
    {
        Vector3 rotationVector = new Vector3(0, 0, 0);

        rotationVector.y = InputManager.Instance.GetCameraRotateAmount();

        transform.eulerAngles += rotationVector * RotationSpeed * Time.deltaTime;
    }

    private void HandleZoom()
    {
        _targetFollowOffset.y += InputManager.Instance.GetCameraZoomAmount() * ZoomAmount;

        _targetFollowOffset.y = Mathf.Clamp(_targetFollowOffset.y, Min_Follow_Y_Offset, Max_Follow_Y_Offset);
        _cinemachineTransposer.m_FollowOffset = Vector3.Lerp(_cinemachineTransposer.m_FollowOffset, _targetFollowOffset, Time.deltaTime * ZoomSpeed);

        //Debug.Log($"Cinemachine {_cinemachineTransposer.m_FollowOffset}, + Target {_targetFollowOffset}");
    }

    public float GetCameraHeight()
    {
        return _targetFollowOffset.y;
    }
}
