using TMPro;
using UnityEngine;

public class GridDebugObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;

    private object _gridObject;

    protected virtual void Update()
    {
        _text.text = _gridObject.ToString();
    }

    public virtual void SetGridObject(object gridObject)
    {
        this._gridObject = gridObject;
    }

    public void SetText(string text)
    {
        _text.text = text;
    }

    
}
