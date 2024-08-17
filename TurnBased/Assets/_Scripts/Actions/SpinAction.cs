using UnityEngine;

public class SpinAction : BaseAction
{
    public delegate void SpinCompleteDelegate();
    private SpinCompleteDelegate _onSpinComplete;

    private float _totalSpinAmount;

    private void Update()
    {
        if (!_isActive) return;

        float spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);

        _totalSpinAmount += spinAddAmount;
        if (_totalSpinAmount >= 360f)
        {
            _isActive = false;
            _onSpinComplete();  
        }
    }

    public void Spin(SpinCompleteDelegate onSpinCompleted)
    {
        this._onSpinComplete = onSpinCompleted;
        _isActive = true;
        _totalSpinAmount = 0f;
        Debug.Log("Spin activated");
    }
}
