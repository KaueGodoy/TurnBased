using TMPro;
using UnityEngine;

public class GridDebugObject : MonoBehaviour
{
    private GridObject _gridObject;

    [SerializeField] private TextMeshPro _text;

    private void Update()
    {
        //_text.text = _gridObject.ToString();
    }

    public void SetGridObject(GridObject gridObject)
    {
        this._gridObject = gridObject;
    }

    public void SetText(string text)
    {
        _text.text = text;
    }

    
}
