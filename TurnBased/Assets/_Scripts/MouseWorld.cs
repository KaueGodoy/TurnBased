using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    public static MouseWorld Instance { get; private set; }

    [SerializeField] private LayerMask _mousePlaneLayerMask;

    private void Awake()
    {
        Instance = this;
    }

    //private void Update()
    //{
    //    transform.position = MouseWorld.GetPosition();
    //}

    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, Instance._mousePlaneLayerMask);
        return raycastHit.point;
    }

}
