using UnityEngine;
using System.Collections;

public class TimerUtilities : MonoBehaviour
{
    #region Inspector variables
    [SerializeField]
    private float _duration;
    #endregion

    #region Private variables
    private float _startTime;
    private float _endTime;
    private float _timeLeft;
    private bool _isPaused = false;
    private float _pauseTime;
    private float _pauseOffset = 0f;
    #endregion

    #region Unity Functions
    void Update()
    {
        if(_timeLeft > 0 && !_isPaused)
        {
            _timeLeft = _endTime - Time.time - _pauseOffset;
            if(_timeLeft < 0)
            {
                _timeLeft = 0;
            }
        }
    }
    #endregion

    #region Timer methods
    public float GetDuration()
    {
        return _duration;
    }

    public float GetTimeLeft()
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

    public void StartTimer(float duration)
    {
        _duration = duration;
        _startTime = Time.time;
        _endTime = _startTime + _duration;
        _timeLeft = duration;
    }

    public void StartTimer()
    {
        _startTime = Time.time;
        _endTime = _startTime + _duration;
        _timeLeft = _duration;
    }

    public void PauseTimer()
    {
        _pauseTime = _pauseOffset + Time.time;
        _isPaused = true;
    }

    public void ResumeTimer()
    {
        _pauseOffset = Time.time - _pauseTime;
        _isPaused = false;
    }

    public float GetTimeLeftInPctDecimal()
    {
        return _timeLeft / _duration;
    }
    #endregion
}
