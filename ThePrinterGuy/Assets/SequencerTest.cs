using UnityEngine;
using System.Collections;

public class SequencerTest : MonoBehaviour {
	
	#region Editor Publics
	[SerializeField] private TaskSequence[] _TaskSequences;
	#endregion
	
	
	#region Privates
	private GUIGameCamera _gGameCam;
	
	//Task Spawning
    private int _sequenceIndex = 0;
	private int _taskCounter = 0;
	
	//Focus Handling
    private int _focusIndex = 0;
	
	//Beat Handling
	private int _beatCounter = 0;
	
	private bool _isSubscribed = false;
	#endregion
	
	#region Delegates & Events
    public delegate void LastNodeAction();
    public static event LastNodeAction OnLastNode;
	
	public delegate void DefaultCamPosAction();
    public static event DefaultCamPosAction OnDefaultCamPos;
	
    public delegate void OnCreateNewNodeAction(string itemName);
    public static event OnCreateNewNodeAction OnCreateNewNode;	//New Task
	
    public delegate void OnTaskAction(int taskIdentifier);
	
    public static event OnTaskAction OnPaperNode;				
    public static event OnTaskAction OnInkNode;
    public static event OnTaskAction OnUraniumRodNode;
    public static event OnTaskAction OnBarometerNode;
	#endregion
	
    #region OnEnable and OnDisable
    void OnEnable()
    {
		GUIGameCamera.OnUpdateAction += UpdateTaskInFocus;
    }

    void OnDisable()
    {
		GUIGameCamera.OnUpdateAction -= UpdateTaskInFocus;
    }
    #endregion
	
	void Awake()
	{
		_gGameCam = GameObject.Find("GUI List").GetComponent<GUIGameCamera>();
	}
	
	void Start()
	{
	
	}
	
	void Update()
	{
		CheckSubscription();
	}
	
	#region Class Methods
	private void CheckSubscription()
	{
		if(!_isSubscribed)
		{
			SubscribeToBeat();
			_isSubscribed = true;
			_beatCounter = 0;
		}
	}
	
	//FIXME: Still use??
	private void SwitchDimmer()
	{		
		switch(_TaskSequences[_sequenceIndex].task.ToString())
		{
		case "Paper":
			//Paper methods
			break;
		case "Ink":
			//Ink Methods
			break;
		case "UraniumRod":
			//Uranium Methods
			break;
		case "Barometer":
			//Barometer Methods
			break;
		}
	}
	
	private void CheckBeat()
	{
		//Check what sequence we are in
		if(_sequenceIndex < _TaskSequences.Length)
		{
			//Check what beat we are on
			if(_beatCounter == 0)
			{
				SimpleSpawnTask();
				_taskCounter++;
				_beatCounter++;
			}
			//Reset beat counter
			else if(_beatCounter >= _TaskSequences[_sequenceIndex].SpawningEvery - 1)
			{
				_beatCounter = 0;
			}
			//Skip spawning this beat
			else
			{
				_beatCounter++;
			}
							
			//Check if last task
			if(_taskCounter >= _TaskSequences[_sequenceIndex].amounts.Length)
			{
				//Reset task counter
				_taskCounter = 0;
				//Increase sequenceCounter
				_sequenceIndex++;
			}				
		}
		
		//Check if last Sequence
		if(_sequenceIndex >= _TaskSequences.Length)
		{
				//Reset counter & index
				_taskCounter = 0;
				_sequenceIndex = 0;
				
				UnSubscribeFromBeat();
				_isSubscribed = false;
				
				//OnLastNode
				if(OnLastNode != null)
					OnLastNode();
		}
	}
	
	private void SubscribeToBeat()
	{
		var temp = _TaskSequences[_sequenceIndex].beat.ToString();
		
		switch(temp)
		{
		case "OnBeat3rd1":
			//
			BeatController.OnBeat3rd1 += CheckBeat;
			break;
		case "OnBeat3rd2":
			//
			BeatController.OnBeat3rd2 += CheckBeat;
			break;
		case "OnBeat3rd3":
			//
			BeatController.OnBeat3rd3 += CheckBeat;
			break;
			
		//4 tact subscriptions
		case "OnBeat4th1":
			//
			BeatController.OnBeat4th1 += CheckBeat;
			break;
		case "OnBeat4th2":
			//
			BeatController.OnBeat4th2 += CheckBeat;
			break;
		case "OnBeat4th3":
			//
			BeatController.OnBeat4th3 += CheckBeat;
			break;
		case "OnBeat4th4":
			//
			BeatController.OnBeat4th4 += CheckBeat;
			break;
			
		//6 tact subscriptions
		case "OnBeat6th1":
			//
			BeatController.OnBeat6th1 += CheckBeat;
			break;
		case "OnBeat6th2":
			//
			BeatController.OnBeat6th2 += CheckBeat;
			break;
		case "OnBeat6th3":
			//
			BeatController.OnBeat6th3 += CheckBeat;
			break;
		case "OnBeat6th4":
			//
			BeatController.OnBeat6th4 += CheckBeat;
			break;
		case "OnBeat6th5":
			//
			BeatController.OnBeat6th5 += CheckBeat;
			break;
		case "OnBeat6th6":
			//
			BeatController.OnBeat6th6 += CheckBeat;
			break;
			
		//6 tact subscriptions
		case "OnBeat8th1":
			//
			BeatController.OnBeat8th1 += CheckBeat;
			break;
		case "OnBeat8th2":
			//
			BeatController.OnBeat8th2 += CheckBeat;
			break;
		case "OnBeat8th3":
			//
			BeatController.OnBeat8th3 += CheckBeat;
			break;
		case "OnBeat8th4":
			//
			BeatController.OnBeat8th4 += CheckBeat;
			break;
		case "OnBeat8th5":
			//
			BeatController.OnBeat8th5 += CheckBeat;
			break;
		case "OnBeat8th6":
			//
			BeatController.OnBeat8th6 += CheckBeat;
			break;
		case "OnBeat8th7":
			//
			BeatController.OnBeat8th7 += CheckBeat;
			break;
		case "OnBeat8th8":
			//
			BeatController.OnBeat8th8 += CheckBeat;
			break;
		default:
			break;
		}
	}
	
	private void UnSubscribeFromBeat()
	{
		var temp = _TaskSequences[_sequenceIndex].beat.ToString();
		
		switch(temp)
		{
		case "OnBeat3rd1":
			//
			BeatController.OnBeat3rd1 -= CheckBeat;
			break;
		case "OnBeat3rd2":
			//
			BeatController.OnBeat3rd2 -= CheckBeat;
			break;
		case "OnBeat3rd3":
			//
			BeatController.OnBeat3rd3 -= CheckBeat;
			break;
			
		//4 tact subscriptions
		case "OnBeat4th1":
			//
			BeatController.OnBeat4th1 -= CheckBeat;
			break;
		case "OnBeat4th2":
			//
			BeatController.OnBeat4th2 -= CheckBeat;
			break;
		case "OnBeat4th3":
			//
			BeatController.OnBeat4th3 -= CheckBeat;
			break;
		case "OnBeat4th4":
			//
			BeatController.OnBeat4th4 -= CheckBeat;
			break;
			
		//6 tact subscriptions
		case "OnBeat6th1":
			//
			BeatController.OnBeat6th1 -= CheckBeat;
			break;
		case "OnBeat6th2":
			//
			BeatController.OnBeat6th2 -= CheckBeat;
			break;
		case "OnBeat6th3":
			//
			BeatController.OnBeat6th3 -= CheckBeat;
			break;
		case "OnBeat6th4":
			//
			BeatController.OnBeat6th4 -= CheckBeat;
			break;
		case "OnBeat6th5":
			//
			BeatController.OnBeat6th5 -= CheckBeat;
			break;
		case "OnBeat6th6":
			//
			BeatController.OnBeat6th6 -= CheckBeat;
			break;
			
		//6 tact subscriptions
		case "OnBeat8th1":
			//
			BeatController.OnBeat8th1 -= CheckBeat;
			break;
		case "OnBeat8th2":
			//
			BeatController.OnBeat8th2 -= CheckBeat;
			break;
		case "OnBeat8th3":
			//
			BeatController.OnBeat8th3 -= CheckBeat;
			break;
		case "OnBeat8th4":
			//
			BeatController.OnBeat8th4 -= CheckBeat;
			break;
		case "OnBeat8th5":
			//
			BeatController.OnBeat8th5 -= CheckBeat;
			break;
		case "OnBeat8th6":
			//
			BeatController.OnBeat8th6 -= CheckBeat;
			break;
		case "OnBeat8th7":
			//
			BeatController.OnBeat8th7 -= CheckBeat;
			break;
		case "OnBeat8th8":
			//
			BeatController.OnBeat8th8 -= CheckBeat;
			break;
		default:
			break;
		}
		
	}
	
	private void SimpleSpawnTask()
	{
		var taskName = _TaskSequences[_sequenceIndex].task.ToString();
		
		//Spawn Task
		if(OnCreateNewNode != null)
			OnCreateNewNode(taskName);
	}
	
    private void UpdateTaskInFocus()
    {
		//Check ??? 
		if(_gGameCam.GetQueueCount() == 0)
		{
			if(OnDefaultCamPos != null)
				OnDefaultCamPos();
		}
		//Check which ask is in focus;
		else if(_focusIndex < _TaskSequences.Length)
		{
			var focusItem = _TaskSequences[_sequenceIndex].task.ToString();
						
	        if(focusItem == "Paper")
	        {
	            if(OnPaperNode != null)
	            {
	                OnPaperNode(_TaskSequences[_sequenceIndex].amounts[_taskCounter] - 1);
	            }
	        }
	        else if(focusItem == "Ink")
	        {
	            if(OnInkNode != null)
	            {
	                OnInkNode(_TaskSequences[_sequenceIndex].amounts[_taskCounter] - 1);
	            }
	        }
	        else if(focusItem == "UraniumRod")
	        {
	            if(OnUraniumRodNode != null)
	            {
	                OnUraniumRodNode(_TaskSequences[_sequenceIndex].amounts[_taskCounter] - 1);
	            }
	        }
	        else if(focusItem == "Barometer")
	        {
	            if(OnBarometerNode != null)
	            {
	                OnBarometerNode(_TaskSequences[_sequenceIndex].amounts[_taskCounter] - 1);
	            }
	        }
            _focusIndex++;
		}			
	}
	#endregion
	
    #region SubClasses
    [System.Serializable]
    public class TaskSequence
    {
		public enum Tasks
		{
			Paper, Ink, UraniumRod, Barometer
		}
        public enum Beats
		{
			//3 tact beats
			OnBeat3rd1,
			OnBeat3rd2,
			OnBeat3rd3,
			//4 tact beats
			OnBeat4th1,
			OnBeat4th2,
			OnBeat4th3,
			OnBeat4th4,
			//6 tact Beats
			OnBeat6th1,
			OnBeat6th2,
			OnBeat6th3,
			OnBeat6th4,
			OnBeat6th5,
			OnBeat6th6,
			//8 tact Beats
			OnBeat8th1,
			OnBeat8th2,
			OnBeat8th3,
			OnBeat8th4,
			OnBeat8th5,
			OnBeat8th6,
			OnBeat8th7,
			OnBeat8th8
		}
		public Tasks task;
		public Beats beat; 
        public int[] amounts;
		public int SpawningEvery;
		public int beatsUntillNextSequence;
    };
    #endregion
}
