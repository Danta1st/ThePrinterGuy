using UnityEngine;
using System.Collections;

public class BpmSequencerItem : MonoBehaviour {
		
    #region SerializeField
    [SerializeField] private string _moduleName;
    [SerializeField] private Vector3 _scaleAmount;
    [SerializeField] private iTween.EaseType _easeTypeMove = iTween.EaseType.easeInSine;
	[SerializeField] Particles _particles;
	[SerializeField] Textures _textures;	
    #endregion
	
	//-----------------------------
    #region Private Variables
    //Prepare for hack!
    private Transform _spawnZonePosition;
    private Transform _deadZonePosition;
    private Transform _failedPos;
    private Transform _perfectPos;
    //---------------------//
    
	//Adjusts the lenght and thereby distance of icons on the sequencer bar
	private int stepmultiplier = 3; 
	
	//Components
    private GUIGameCamera _guiGameCameraScript;
    private TempoManager _tempoManagerScript;
    private GreenZone _greenZoneScript;
	private GameObject _dynamicObjects;
	
    private string _statusZone = "";
    private int _zone = 0;
	
	//Tempo wobbler variables
	private float _ms;
    private float _TaskKillTime = 0.4f;
	
	//Movement Variables
	private StepLenghts _stepLenghts = new StepLenghts();
	private Steps _steps = new Steps();
    private Vector3 _destinationPosition;
    private Vector3 _partialPosition;
    private Transform[] _path = new Transform[2];
    #endregion
	
	public string GetTaskName()
	{
		return _moduleName;
	}

    #region Delegates and Events
    public delegate void FailedAction();
    public static event FailedAction OnFailed;
	
	public delegate void FailedActionWithItem(string type);
    public static event FailedActionWithItem OnFailedWithItem;
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
		
		if(_statusZone == "Green")
			KillComplete();
	}
	
    // Use this for initialization
    void Awake()
    {
		_dynamicObjects = GameObject.Find("Dynamic Objects");
    	_spawnZonePosition 	= GameObject.Find("SpawnZone").transform; //GameObject.Find("SpawnZone").transform;
    	_deadZonePosition 	= GameObject.Find("DeadZone").transform;

        _perfectPos = GameObject.Find("PerfectPos").transform;
        _failedPos = GameObject.Find("FailedPos").transform;

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
			BeatController.OnAll4Beats += StartScalePaper;
			//Horizontal movement
			BeatController.OnAll4Beats += StartMovePaper;
			break;
		case "Ink":
			//Vertical rhythm
			BeatController.OnAll4Beats += StartScaleInk;
			//Horizontal movement
			BeatController.OnAll4Beats += StartMoveInk;
			break;
		case "UraniumRod":
			//Vertical rhythm
			BeatController.OnAll8Beats += StartScaleUranium;
			//Horizontal movement
			BeatController.OnAll8Beats += StartMoveUranium;
			break;
		case "Barometer":

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
			BeatController.OnAll4Beats -= StartScalePaper;
			//Horizontal movement
			BeatController.OnAll4Beats -= StartMovePaper;
			break;
		case "Ink":
			BeatController.OnAll4Beats -= StartScaleInk;
			//Horizontal movement
			BeatController.OnAll4Beats -= StartMoveInk;
			break;
		case "UraniumRod":
			BeatController.OnAll8Beats -= StartScaleUranium;
			//Horizontal movement
			BeatController.OnAll8Beats -= StartMoveUranium;
			break;
		case "Barometer":
			
			break;
		}
	}
	
	//Punch and wobble!
    public void StartScalePaper()
    {
    	float scaleTime = _stepLenghts.fourths * 5.0f;
        iTween.PunchScale(gameObject, iTween.Hash("amount", _scaleAmount, "time", scaleTime, "easeType", iTween.EaseType.linear));
    }
    
    public void StartScaleInk()
    {
    	float scaleTime = _stepLenghts.fourths * 5.0f;
        iTween.PunchScale(gameObject, iTween.Hash("amount", _scaleAmount, "time", scaleTime, "easeType", iTween.EaseType.linear));
    }
    
    public void StartScaleUranium()
    {
    	float scaleTime = _stepLenghts.eights * 5.0f;
        iTween.PunchScale(gameObject, iTween.Hash("amount", _scaleAmount, "time", scaleTime, "easeType", iTween.EaseType.linear));
    }
		
	//MoveMethods - crap method - TODO: could this be generalised?
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
			{
				_statusZone = "Red";
			}
			else if(_steps.stepsMoved == yellow)
			{
				_statusZone = "Yellow";
				_greenZoneScript.YellowOn();
			}
			else if(_steps.stepsMoved == green)
			{
				_statusZone = "Green";
				_greenZoneScript.GreenOn();
//                KillTaskPerfect();
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
					
					if(OnFailedWithItem != null)
						OnFailedWithItem(_moduleName);
					
//					_guiGameCameraScript.EndZone(gameObject, false);
				}
				//TODO: Insert end dissapear method
                KillTaskFailed();
			}
			
			ChangeTexture();
			break;
		case "Ink":
			red = _steps.maxFourths-2;
			yellow = _steps.maxFourths-1;
			green = _steps.maxFourths;
			dead = _steps.maxFourths+1;
			
			if(_steps.stepsMoved == red)
			{
				_statusZone = "Red";
			}
			else if(_steps.stepsMoved == yellow)
			{
				_statusZone = "Yellow";
			}
			else if(_steps.stepsMoved == green)
			{
				_statusZone = "Green";
				_greenZoneScript.GreenOn();
//                KillTaskPerfect();
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
					
					if(OnFailedWithItem != null)
						OnFailedWithItem(_moduleName);
					
//					_guiGameCameraScript.EndZone(gameObject, false);
				}
				//TODO: Insert end dissapear method
                KillTaskFailed();
			}
			ChangeTexture();
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
//                KillTaskPerfect();
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
					
					if(OnFailedWithItem != null)
						OnFailedWithItem(_moduleName);
					
//					_guiGameCameraScript.EndZone(gameObject, false);
				}
				//TODO: Insert end dissapear method
                KillTaskFailed();
			}
			ChangeTexture();
			break;
		case "Barometer":
			break;
		default:
			Debug.LogWarning (gameObject.name+" received wrong moduleName. Subscribing nothing. Check _moduleName on prefab");
			break;
		}
	}
	
	//Method for changing the texture of the item
	private void ChangeTexture()
	{
        if(_statusZone == "Red")
        {
			renderer.material.mainTexture = _textures.red;
        }
        else if(_statusZone == "Yellow")
        {
			renderer.material.mainTexture = _textures.yellow;
        }
        else if(_statusZone == "Green")
        {
			renderer.material.mainTexture = _textures.green;
        }
		else			
			renderer.material.mainTexture = _textures.red;
	}
	
	//Method for identifying which zone the task is currently in
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
	
	//Methods for killing the task
    private void KillTaskFailed()
    {
        iTween.MoveTo(gameObject, iTween.Hash("position", _failedPos, "time", _TaskKillTime, "easetype", iTween.EaseType.linear,
            "oncomplete", "KillFailed", "oncompletetarget", gameObject));
        iTween.ScaleTo(gameObject, iTween.Hash("scale", new Vector3(0.0f, 0.0f, 0.0f), "time", _TaskKillTime, "easetype", iTween.EaseType.linear));
    }

    private void KillTaskPerfect()
    {
        iTween.MoveTo(gameObject, iTween.Hash("position", _perfectPos, "time", _TaskKillTime, "easetype", iTween.EaseType.linear,
            "oncomplete", "KillComplete", "oncompletetarget", gameObject));
        iTween.ScaleTo(gameObject, iTween.Hash("scale", new Vector3(0.0f, 0.0f, 0.0f), "time", _TaskKillTime, "easetype", iTween.EaseType.linear));
    }

	private void KillFailed()
	{
		_guiGameCameraScript.EndZone(gameObject, false);
        InstantiateParticles(_particles.failed, gameObject);
        Destroy(gameObject);
	}

    private void KillComplete()
 {
     //TODO: implement kill particle task
        InstantiateParticles(_particles.completed, gameObject);
        Destroy(gameObject);
 }

	//Method for instantiating particles
	private void InstantiateParticles(GameObject particles, GameObject posRotGO)
	{
		if(particles != null)
		{
			foreach(Transform child in posRotGO.transform)
			{
				if(child.name.Equals("ParticlePos") && particles != null)
				{
					//Instantiate Particle prefab. Rotation solution is a HACK
					GameObject tempParticles = (GameObject) Instantiate(particles, child.position, Quaternion.identity);
					//Child to DynamicObjects
					tempParticles.transform.parent = _dynamicObjects.transform;
					return;
				}				
			}
			//Instantiate Particle prefab. Rotation solution is a HACK
			GameObject tempParticles1 = (GameObject) Instantiate(particles, posRotGO.transform.position, Quaternion.identity);
			//Child to DynamicObjects
			tempParticles1.transform.parent = _dynamicObjects.transform;
		}
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
	
    [System.Serializable]
    public class Particles
    {
		public GameObject completed;
        public GameObject failed;
    };
	
    [System.Serializable]
    public class Textures
    {
		public Texture2D red;
		public Texture2D yellow;
		public Texture2D green;
	}
}
