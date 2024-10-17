using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private GameObject _selectedGameObject;

    public void Show(Material material)
    {
        _meshRenderer.enabled = true;
        _meshRenderer.material = material;  
    }

    public void Hide()
    {
        _meshRenderer.enabled = false;
    }

    public void ShowSelected()
    {
        _selectedGameObject.SetActive(true);
    }

    public void HideSelected()
    {
        _selectedGameObject.SetActive(false);
    }
}