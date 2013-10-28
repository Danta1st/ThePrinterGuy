using UnityEngine;
using System.Collections;

public class TimerUtilities : MonoBehaviour
{
    [SerializeField]
    private float _duration = 60;

    private float _startTime;
    private float _endTime;

    private bool _isElapsed = false;

    // Use this for initialization
    void Start()
    {
        _startTime = Time.time;
        _endTime = _startTime + _duration;
    }

    // Update is called once per frame
    void Update()
    {
        if(_isElapsed == false)
        {
            if(Time.time >= _endTime)
            {
                _isElapsed = true;
            }
        }
    }

    public float getTimeLeft()
    {
        float timeLeft;
        if (!_isElapsed)
        {
            timeLeft = _endTime - Time.time;
        }
        else
        {
            timeLeft = 0;
        }

        return timeLeft;
    }

    public void DestroyTimer()
    {
        Destroy(this);
    }

    public void ExtendTimer(float extensionTime)
    {
        _endTime += extensionTime;
    }

    public void DecreaseTimer(float decreaseTime)
    {
        _endTime -= decreaseTime;
    }

    public bool isElapsed()
    {
        return _isElapsed;
    }
}
