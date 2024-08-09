using Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    private const float Min_Follow_Y_Offset = 2f;
    private const float Max_Follow_Y_Offset = 12f;

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

    private void Start()
    {
        _cinemachineTransposer = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        _targetFollowOffset = _cinemachineTransposer.m_FollowOffset;
    }

    private void Update()
    {
        Vector3 inputMoveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.z = +1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x = -1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.z = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x = +1f;
        }

        Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
        transform.position += moveVector * MoveSpeed * Time.deltaTime;

        Vector3 rotationVector = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.E))
        {
            rotationVector.y = -1f;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            rotationVector.y = +1f;
        }

        transform.eulerAngles += rotationVector * RotationSpeed * Time.deltaTime;

        if (Input.mouseScrollDelta.y > 0)
        {
            _targetFollowOffset.y -= ZoomAmount;
        }

        if (Input.mouseScrollDelta.y < 0)
        {
            _targetFollowOffset.y += ZoomAmount;
        }

        _targetFollowOffset.y = Mathf.Clamp(_targetFollowOffset.y, Min_Follow_Y_Offset, Max_Follow_Y_Offset);
        _cinemachineTransposer.m_FollowOffset = Vector3.Lerp(_cinemachineTransposer.m_FollowOffset, _targetFollowOffset, Time.deltaTime * ZoomSpeed);

        Debug.Log($"Cinemachine {_cinemachineTransposer.m_FollowOffset}, + Target {_targetFollowOffset}");

    }
}