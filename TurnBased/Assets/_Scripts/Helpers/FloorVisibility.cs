using System.Collections.Generic;
using UnityEngine;

public class FloorVisibility : MonoBehaviour
{
    private Renderer[] _rendererArray;
    private int _floor;
    [SerializeField] private bool _dynamicFloorPosition;
    [SerializeField] private List<Renderer> _ignoreRendererList;

    private void Awake()
    {
        _rendererArray = GetComponentsInChildren<Renderer>(true);
    }

    private void Start()
    {
        _floor = LevelGrid.Instance.GetFloor(transform.position);

        if (_floor == 0 && !_dynamicFloorPosition)
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if (_dynamicFloorPosition)
        {
            _floor = LevelGrid.Instance.GetFloor(transform.position);
        }

        float cameraHeight = CameraController.Instance.GetCameraHeight();

        float floorHeightOffset = 2f;
        bool showObj = cameraHeight > LevelGrid.FloorHeight * _floor + floorHeightOffset;

        if (showObj || _floor == 0)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        foreach (Renderer renderer in _rendererArray)
        {
            if (_ignoreRendererList.Contains(renderer)) continue;

            renderer.enabled = true;
        }
    }

    private void Hide()
    {
        foreach (Renderer renderer in _rendererArray)
        {
            if (_ignoreRendererList.Contains(renderer)) continue;

            renderer.enabled = false;
        }
    }

}
