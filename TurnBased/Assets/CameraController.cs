using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _rotationSpeed = 50f;
    public float MoveSpeed { get { return _moveSpeed; } set { value = _moveSpeed; } }
    public float RotationSpeed { get { return _rotationSpeed; } set { value = _rotationSpeed; } }

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
    }
}
