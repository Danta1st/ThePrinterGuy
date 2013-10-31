using UnityEngine;
using System.Collections;

public class TimerUtilities : MonoBehaviour
{
    #region Inspector variables
    [SerializeField]
    private float _duration;
	[SerializeField]
    private float _tickRate = 1;
	
    #endregion

    #region Private variables
    private float _startTime;
    private float _endTime;
	[SerializeField]
    private float _timeLeft;
    private bool _isPaused = false;
	private bool _manualTimer;
	
    private float _pauseTime;
    private float _pauseOffset = 0f;
	
    #endregion

    #region Unity Functions
    void Update()
    {
        if(_timeLeft > 0 && !_isPaused && !_manualTimer)
        {
            _timeLeft = _endTime - (Time.time * _tickRate) + _pauseOffset * _tickRate;
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
	
	public void SetTickRate(float tickrate)
	{
		_tickRate = tickrate;	
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
	
	public void StartTimer(float duration, float tickrate)
    {
		_tickRate = tickrate;
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
        _pauseTime = Time.time;
        _isPaused = true;
    }

    public void ResumeTimer()
    {
        _pauseOffset = _pauseOffset + (Time.time - _pauseTime);
        _isPaused = false;
    }

    public float GetTimeLeftInPctDecimal()
    {
        return _timeLeft / _duration;
    }
    #endregion
}
