using UnityEngine;
using System.Collections;

public class ActionSequencerItem : MonoBehaviour
{
    #region SerializeField
    [SerializeField]
    private string _moduleName;
    [SerializeField]
    private iTween.EaseType _easeTypeActionSequencerItem;
    #endregion

    #region Private Variables
    //Prepare for hack!
    private Vector3 _deadZonePosition;
    private Vector3 _bufferZonePosition;
    private Vector3 _greenZonePosition;
    private Vector3 _yellowZonePosition;
    private Vector3 _redZonePosition;
    //---------------------//
    
    private GUIGameCamera _guiGameCameraScript;
    private ActionSequencerZone _actionSequencerScript;
    private TempoManager _tempoManagerScript;
    private GreenZone _greenZoneScript;
    private string _statusZone = "";
    private int _zone = 0;
	
	private float _time;
	private float _ms;
    private bool _once = false;
    private bool _isBack = false;
    private Vector3 _destinationPosition;
    #endregion

    #region Delegates and Events
    public delegate void FailedAction();
    public static event FailedAction OnFailed;
    #endregion
	
	void OnEnable()
	{
		if(_moduleName == "Ink")
		{
			TempoManager.OnInkTempo += StartTween;
		}
		else if(_moduleName == "Paper")
		{
			TempoManager.OnPaperTempo += StartTween;
		}
		else if(_moduleName == "UraniumRod")
		{
			TempoManager.OnUraniumRodTempo += StartTween;
		}
		else if(_moduleName == "Barometer")
		{
			TempoManager.OnBarometerTempo += StartTween;
		}
	}
	
	void OnDisable()
	{
		if(_moduleName == "Ink")
		{
			TempoManager.OnInkTempo -= StartTween;
		}
		else if(_moduleName == "Paper")
		{
			TempoManager.OnPaperTempo -= StartTween;
		}
		else if(_moduleName == "UraniumRod")
		{
			TempoManager.OnUraniumRodTempo -= StartTween;
		}
		else if(_moduleName == "Barometer")
		{
			TempoManager.OnBarometerTempo -= StartTween;
		}
	}

    // Use this for initialization
    void Start()
    {
    	_deadZonePosition = GameObject.Find("DeadZone").transform.position;
    	_bufferZonePosition = GameObject.Find("BufferZone").transform.position;
    	_greenZonePosition = GameObject.Find("GreenZone").transform.position;
    	_yellowZonePosition = GameObject.Find("YellowZone").transform.position;
    	_redZonePosition = GameObject.Find("RedZone").transform.position;
    	
    	_greenZoneScript = GameObject.Find("GreenZone").GetComponent<GreenZone>();
    
        _guiGameCameraScript = GameObject.Find("GUI List").GetComponent<GUIGameCamera>();
        _tempoManagerScript = GameObject.Find("GUI List").GetComponent<TempoManager>();
        _destinationPosition = GameObject.Find("DeadZone").transform.position;
       
		if(_moduleName == "Ink")
		{
			_time = _tempoManagerScript.GetInkTime();
			_ms = _tempoManagerScript.GetInkMs();
		}
		else if(_moduleName == "Paper")
		{
			_time = _tempoManagerScript.GetPaperTime();
			_ms = _tempoManagerScript.GetPaperMs();
		}
		else if(_moduleName == "UraniumRod")
		{
			_time = _tempoManagerScript.GetUraniumRodTime();
			_ms = _tempoManagerScript.GetUraniumRodMs();
		}
		else if(_moduleName == "Barometer")
		{
			_time = _tempoManagerScript.GetBarometerTime();
			_ms = _tempoManagerScript.GetBarometerMs();
		}
    }

    // Update is called once per frame
    void Update()
    {
		if(transform.position.x < _bufferZonePosition.x)
		{
			_statusZone = "Buffer";
		}
		if(transform.position.x < _redZonePosition.x)
		{
			_statusZone = "Red";
		}
		if(transform.position.x < _yellowZonePosition.x)
		{
			_statusZone = "Yellow";
		}
		if(transform.position.x < _greenZonePosition.x)
		{
			_statusZone = "Green";
			_greenZoneScript.GreenOn();
		}
		if(transform.position.x < _greenZonePosition.x-20)
		{
			_greenZoneScript.GreenOff();
		}
		if(transform.position.x <= _deadZonePosition.x)
		{
			_statusZone = "Dead";
			if(OnFailed != null)
			{
				OnFailed();
			}
			_guiGameCameraScript.EndZone(gameObject);
		}
    }

//    void OnTriggerEnter(Collider other)
//    {
//        if(other.gameObject.tag == "SequencerZone")
//        {
//            _actionSequencerScript = other.gameObject.GetComponent<ActionSequencerZone>();
//            _statusZone = _actionSequencerScript.GetZone();
//
//            if(_statusZone == "Dead")
//            {
//                if(OnFailed != null)
//                {
//                    OnFailed();
//                }
//                _guiGameCameraScript.EndZone(gameObject);
//            }
//        }
//    }

    public void StartTween()
    {
//    	if(_once == false)
//    	{
//    		_once = true;
			//iTween.Stop(gameObject);
        	iTween.PunchScale(gameObject, iTween.Hash("amount", new Vector3(0,20,0), "time", _ms));
			iTween.MoveTo(gameObject, iTween.Hash("position", _destinationPosition, "time", _time,
                                                    "easeType", _easeTypeActionSequencerItem));
//    	}
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
