using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    private void Update()
    {
        //Debug.Log(Input.mousePosition);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.Log(Physics.Raycast(ray));
    }
}
