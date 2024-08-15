using UnityEngine;

public class SpinAction : MonoBehaviour
{
    private bool _startSpinning;

    private void Update()
    {
        if (_startSpinning)
        {
            float spinAddAmount = 360f * Time.deltaTime;
            transform.eulerAngles += new Vector3(0, spinAddAmount, 0);
        }
    }

    public void Spin()
    {
        _startSpinning = true;
    }
}
