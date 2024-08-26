using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{

    private void Start()
    {
        UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;
        Hide();
    }

    private void UnitActionSystem_OnBusyChanged(object sender, bool busy)
    {
        if (busy)
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
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
