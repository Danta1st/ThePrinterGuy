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
        return Time.time - _endTime;
    }

    public void DestroyTimer()
    {
        Destroy(this);
    }

}
