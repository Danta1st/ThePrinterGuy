using UnityEngine;
using System.Collections;

public class TimerUtilities : MonoBehaviour
{
    [SerializeField]
    private float _duration = 60;

    private float _startTime;
    private float _endTime;
    private float _timeLeft;
    private bool _isPaused = true;

    private bool _isElapsed = false;

    // Use this for initialization
    void Start()
    {
        _startTime = Time.time;
        _endTime = _startTime + _duration;
        _timeLeft = _duration;
        StartCoroutine(Timer());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public float getTimeLeft()
    {
        return _timeLeft;
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

    public bool IsElapsed()
    {
        return _isElapsed;
    }

    public void pauseTimer()
    {
        _pauseTimer = true;
    }

    IEnumerator Timer()
    {
        while(_timeLeft > 0)
        {
            if(_pauseTimer == false)
            {
                _timeLeft = _endTime - Time.time;
                yield return new WaitForSeconds(1f);
            }
            else
            {
                yield break;
            }
        }
    }


}
