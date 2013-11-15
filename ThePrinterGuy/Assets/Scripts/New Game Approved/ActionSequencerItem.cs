using UnityEngine;
using System.Collections;

public class ActionSequencerItem : MonoBehaviour
{
    #region Private Variables
    private GUIGameCamera _guiGameCameraScript;
    private ActionSequencerZone _actionSequencerScript;
    private string _statusZone = "";
    private int _zone = 0;

    private float _speed = 40.0f;
    private float _startTime;
    private float _journeyLength;
    private bool _back = false;
    private Vector3 _startSize;
    private Vector3 _newSize;
    #endregion

    #region Delegates and Events
    public delegate void FailedAction();
    public static event FailedAction OnFailed;
    #endregion

    // Use this for initialization
    void Start()
    {
        _guiGameCameraScript = GameObject.Find("GUI List").GetComponent<GUIGameCamera>();

        _startSize = transform.localScale;
        _newSize = new Vector3(transform.localScale.x*1.2f, transform.lossyScale.y*1.2f, transform.localScale.z);

        _startTime = Time.time;
        _journeyLength = Vector3.Distance(_startSize, _newSize);
    }

    // Update is called once per frame
    void Update()
    {
        ScaleSize();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "SequencerZone")
        {
            _actionSequencerScript = other.gameObject.GetComponent<ActionSequencerZone>();
            _statusZone = _actionSequencerScript.GetZone();

            if(_statusZone == "Dead")
            {
                if(OnFailed != null)
                {
                    OnFailed();
                }
                _guiGameCameraScript.EndZone(gameObject);
            }
        }
    }

    private void ScaleSize()
    {
        float _distCovered = (Time.time - _startTime) * _speed;
        float _fracJourney = _distCovered / _journeyLength;

        if(_fracJourney > 1.0f)
        {
            _startTime = Time.time;
            _fracJourney = 0.0f;
            if(_back)
            {
                _back = false;
            }
            else
            {
                _back = true;
            }
        }

        if(_back)
        {
            transform.localScale = Vector3.Lerp(_newSize, _startSize, _fracJourney);
        }
        else
        {
            transform.localScale = Vector3.Lerp(_startSize, _newSize, _fracJourney);
        }
    }

    public int GetZoneStatus()
    {
        if(_statusZone == "Red")
        {
            _zone = 1;
        }
        else if(_statusZone == "Yellow")
        {
            _zone = 2;
        }
        else if(_statusZone == "Green")
        {
            _zone = 3;
        }

        return _zone;
    }
}
