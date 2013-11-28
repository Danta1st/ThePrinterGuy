using UnityEngine;
using System.Collections;

public class BpmSequencerItem : MonoBehaviour {
		
    #region SerializeField
    [SerializeField] private string _moduleName;
    [SerializeField] private iTween.EaseType _easeTypeMove = iTween.EaseType.easeInSine;
    #endregion
	
	//-----------------------------
    #region Private Variables
    //Prepare for hack!
    private Transform _spawnZonePosition;
    private Transform _deadZonePosition;
    private Vector3 _bufferZonePosition;
    private Vector3 _greenZonePosition;
    private Vector3 _yellowZonePosition;
    private Vector3 _redZonePosition;
    //---------------------//
    
	//Adjusts the lenght and thereby distance of icons on the sequencer bar
	private int stepmultiplier = 3; 
	
	//Components
    private GUIGameCamera _guiGameCameraScript;
    private TempoManager _tempoManagerScript;
    private GreenZone _greenZoneScript;
	
    private string _statusZone = "";
    private int _zone = 0;
	
	//Tempo wobbler variables
	private float _ms;
	
	//Movement Variables
	private StepLenghts _stepLenghts = new StepLenghts();
	private Steps _steps = new Steps();
    private Vector3 _destinationPosition;
    private Vector3 _partialPosition;
    private Transform[] _path = new Transform[2];
    #endregion

    #region Delegates and Events
    public delegate void FailedAction();
    public static event FailedAction OnFailed;
    #endregion
	
	void OnEnable()
{
		//Initialise steps
		InitializeStepLenghts(stepmultiplier);
		InitializeStepCounts(stepmultiplier);
		
		SubscribeBeatAndScale(_moduleName);
	}
	void OnDisable()
	{
		UnsubscribeBeatAndScale(_moduleName);
	}
	void OnDestroy()
	{
		UnsubscribeBeatAndScale(_moduleName);		
	}
	
    // Use this for initialization
    void Awake()
    {		
    	_spawnZonePosition 	= GameObject.Find("SpawnZone").transform; //GameObject.Find("SpawnZone").transform;
    	_deadZonePosition 	= GameObject.Find("DeadZone").transform;
    	
    	_greenZoneScript = GameObject.Find("GreenZone").GetComponent<GreenZone>();
    
        _guiGameCameraScript = GameObject.Find("GUI List").GetComponent<GUIGameCamera>();
        _tempoManagerScript = GameObject.Find("GUI List").GetComponent<TempoManager>();
        //_destinationPosition = GameObject.Find("DeadZone").transform.position;
       
		//Adjust path
       _path[0] = _spawnZonePosition;
       _path[1] = _deadZonePosition;
       _partialPosition = _spawnZonePosition.position;
       
		//Set tempo times
		if(_moduleName == "Ink")
		{
			_ms = _tempoManagerScript.GetInkMs();
		}
		else if(_moduleName == "Paper")
		{
			_ms = _tempoManagerScript.GetPaperMs();
		}
		else if(_moduleName == "UraniumRod")
		{
			_ms = _tempoManagerScript.GetUraniumRodMs();
		}
		else if(_moduleName == "Barometer")
		{
			_ms = _tempoManagerScript.GetBarometerMs();
		}
    }
	
    // Update is called once per frame
    void Update()
    {
		CheckZone();
    }
	
	//Subscription Methods
	private void SubscribeBeatAndScale(string moduleName)
	{
		switch(moduleName)
		{
		case "Paper":
			//Vertical rhythm
			TempoManager.OnPaperTempo += StartScale;
			//Horizontal movement
			BeatController.OnBeat4th1 += StartMovePaper;
			BeatController.OnBeat4th2 += StartMovePaper;
			BeatController.OnBeat4th3 += StartMovePaper;
			BeatController.OnBeat4th4 += StartMovePaper;
			break;
		case "Ink":
			//Vertical rhythm
			TempoManager.OnInkTempo += StartScale;
			//Horizontal movement
			BeatController.OnBeat4th1 += StartMoveInk;
			BeatController.OnBeat4th2 += StartMoveInk;
			BeatController.OnBeat4th3 += StartMoveInk;
			BeatController.OnBeat4th4 += StartMoveInk;
			break;
		case "UraniumRod":
			//Vertical rhythm
			TempoManager.OnUraniumRodTempo += StartScale;
			//Horizontal movement
			BeatController.OnBeat8th1 += StartMoveUranium;
			BeatController.OnBeat8th2 += StartMoveUranium;
			BeatController.OnBeat8th3 += StartMoveUranium;
			BeatController.OnBeat8th4 += StartMoveUranium;
			BeatController.OnBeat8th5 += StartMoveUranium;
			BeatController.OnBeat8th6 += StartMoveUranium;
			BeatController.OnBeat8th7 += StartMoveUranium;
			BeatController.OnBeat8th8 += StartMoveUranium;
			break;
		case "Barometer":
			TempoManager.OnBarometerTempo += StartScale;
			break;
		default:
			Debug.LogWarning (gameObject.name+" received wrong moduleName. Subscribing nothing. Check _moduleName on prefab");
			break;
		}
	}
	
	private void UnsubscribeBeatAndScale(string moduleName)
	{
		switch(moduleName)
		{
		case "Paper":			
			//Vertical rhythm
			TempoManager.OnPaperTempo -= StartScale;
			//Horizontal movement
			BeatController.OnBeat4th1 -= StartMovePaper;
			BeatController.OnBeat4th2 -= StartMovePaper;
			BeatController.OnBeat4th3 -= StartMovePaper;
			BeatController.OnBeat4th4 -= StartMovePaper;
			break;
		case "Ink":
			TempoManager.OnInkTempo -= StartScale;
			//Horizontal movement
			BeatController.OnBeat4th1 -= StartMoveInk;
			BeatController.OnBeat4th2 -= StartMoveInk;
			BeatController.OnBeat4th3 -= StartMoveInk;
			BeatController.OnBeat4th4 -= StartMoveInk;
			break;
		case "UraniumRod":
			TempoManager.OnUraniumRodTempo -= StartScale;
			//Horizontal movement
			BeatController.OnBeat8th1 -= StartMoveUranium;
			BeatController.OnBeat8th2 -= StartMoveUranium;
			BeatController.OnBeat8th3 -= StartMoveUranium;
			BeatController.OnBeat8th4 -= StartMoveUranium;
			BeatController.OnBeat8th5 -= StartMoveUranium;
			BeatController.OnBeat8th6 -= StartMoveUranium;
			BeatController.OnBeat8th7 -= StartMoveUranium;
			BeatController.OnBeat8th8 -= StartMoveUranium;
			break;
		case "Barometer":
			TempoManager.OnBarometerTempo -= StartScale;
			break;
		}
	}
	
	//Punch and wobble!
    public void StartScale()
    {
        iTween.PunchScale(gameObject, iTween.Hash("amount", new Vector3(0,20,0), "time", _ms));
    }
		
	//MoveMethods - could this be generalised?
	public void StartMovePaper()
	{
		//Calibrate ze movetime
		float moveTime = _stepLenghts.fourths * 0.98f;
		
		//Calibrate le positiones
		_steps.currentPosOnPath += _stepLenghts.fourths;
		_partialPosition = iTween.PointOnPath(_path, _steps.currentPosOnPath);
		
		//Move the darn thing		
		iTween.MoveTo(gameObject, iTween.Hash("position", _partialPosition, "time", moveTime,
                                                    "easeType", iTween.EaseType.easeOutBack));
		//Increase step count!		
		_steps.stepsMoved++;
	}
	
	public void StartMoveInk()
	{
		//Calibrate ze movetime
		float moveTime = _stepLenghts.fourths * 0.98f;
		
		//Calibrate le positiones
		_steps.currentPosOnPath += _stepLenghts.fourths;
		_partialPosition = iTween.PointOnPath(_path, _steps.currentPosOnPath);
		
		//Move the darn thing		
		iTween.MoveTo(gameObject, iTween.Hash("position", _partialPosition, "time", moveTime,
                                                    "easeType", iTween.EaseType.easeOutBack));
		//Increase step count!		
		_steps.stepsMoved++;
	}
	
	public void StartMoveUranium()
	{
		//Calibrate ze movetime
		float moveTime = _stepLenghts.eights * 0.98f;
		
		//Calibrate le positiones
		_steps.currentPosOnPath += _stepLenghts.eights;
		_partialPosition = iTween.PointOnPath(_path, _steps.currentPosOnPath);
		
		//Move the darn thing		
		iTween.MoveTo(gameObject, iTween.Hash("position", _partialPosition, "time", moveTime,
                                                    "easeType", _easeTypeMove));
		//Increase step count!
		_steps.stepsMoved++;
	}
	//TODO: Barometer method
	
	private bool isDead = false;
	private void CheckZone()
	{
		var red = _steps.maxFourths-2;
		var yellow = _steps.maxFourths-1;
		var green = _steps.maxFourths;
		var dead = _steps.maxFourths+1;
		
		switch(_moduleName)
		{
		case "Paper":			
			if(_steps.stepsMoved == red)
				_statusZone = "Red";
			else if(_steps.stepsMoved == yellow)
				_statusZone = "Yellow";
			else if(_steps.stepsMoved == green)
			{
				_statusZone = "Green";
				_greenZoneScript.GreenOn();
			}
			else if(_steps.stepsMoved >= dead)
			{
				if(!isDead)
				{
					isDead = true;
					_statusZone = "Dead";
					_greenZoneScript.GreenOff();
					
					if(OnFailed != null)				
						OnFailed();
					
					_guiGameCameraScript.EndZone(gameObject, false);
				}
				//TODO: Insert end dissapear method
			}		
			break;
		case "Ink":
			red = _steps.maxFourths-2;
			yellow = _steps.maxFourths-1;
			green = _steps.maxFourths;
			dead = _steps.maxFourths+1;
			
			if(_steps.stepsMoved == red)
				_statusZone = "Red";
			else if(_steps.stepsMoved == yellow)
				_statusZone = "Yellow";
			else if(_steps.stepsMoved == green)
			{
				_statusZone = "Green";
				_greenZoneScript.GreenOn();
			}
			else if(_steps.stepsMoved >= dead)
			{
				if(!isDead)
				{
					isDead = true;
					_statusZone = "Dead";
					_greenZoneScript.GreenOff();
					
					if(OnFailed != null)				
						OnFailed();
					
					_guiGameCameraScript.EndZone(gameObject, false);
				}
				//TODO: Insert end dissapear method
			}	
			break;
		case "UraniumRod":
			red = _steps.maxEights-2;
			yellow = _steps.maxEights-1;
			green = _steps.maxEights;
			dead = _steps.maxEights+1;
			
			if(_steps.stepsMoved == red)
				_statusZone = "Red";
			else if(_steps.stepsMoved == yellow)
				_statusZone = "Yellow";
			else if(_steps.stepsMoved == green)
			{
				_statusZone = "Green";
				_greenZoneScript.GreenOn();
			}
			else if(_steps.stepsMoved >= dead)
			{
				if(!isDead)
				{
					isDead = true;
					_statusZone = "Dead";
					_greenZoneScript.GreenOff();
					
					if(OnFailed != null)				
						OnFailed();
					
					_guiGameCameraScript.EndZone(gameObject, false);
				}
				//TODO: Insert end dissapear method
			}	
			break;
		case "Barometer":
			break;
		default:
			Debug.LogWarning (gameObject.name+" received wrong moduleName. Subscribing nothing. Check _moduleName on prefab");
			break;
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
	
	//Initialization
	private void InitializeStepLenghts(int _stepMultiplier)
	{
		float skalar = (float) _stepMultiplier;
		
		//Calculate lengths
		_stepLenghts.thirds 	= 1f / ((3f * skalar));
		_stepLenghts.fourths 	= 1f / ((4f * skalar));
		_stepLenghts.sixths 	= 1f / ((6f * skalar));		
		_stepLenghts.eights 	= 1f / ((8f * skalar));
	}
	
	private void InitializeStepCounts(int _stepMultiplier)
	{
		int skalar = _stepMultiplier;
		
		//Calculate lengths
		_steps.maxThirds 	= (3 * skalar);
		_steps.maxFourths 	= (4 * skalar);
		_steps.maxSixths 	= (6 * skalar);
		_steps.maxEights 	= (8 * skalar);
	}
	
	//TODO: Return step lenght based on index
	public float GetStepLenght(int step)
	{
		//TODO: Return step lenght based on index
		return 0.0f;
	}
	
    [System.Serializable]
    public class StepLenghts
    {
        public float thirds;
		public float fourths;
		public float sixths;
		public float eights;
    };
	
    [System.Serializable]
    public class Steps
    {
        public float currentPosOnPath = 0;
		public int stepsMoved = 0;
		
		//Maximum values
        public int maxThirds = 0;
        public int maxFourths = 0;
        public int maxSixths = 0;
        public int maxEights = 0;
    };
}
