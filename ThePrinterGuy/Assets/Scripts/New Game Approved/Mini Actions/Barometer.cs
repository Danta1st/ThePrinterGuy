using UnityEngine;
using System.Collections;

public class Barometer : MonoBehaviour 
{
    #region Editor Publics
	[SerializeField] Barometers[] _barometers;
    #endregion

    #region Privates
	private float _normalRotationSpeed = 45;
	private float _brokenRotationSpeed = 720;
    private GenericSoundScript GSS;
    #endregion

    #region Delegates & Events
	public delegate void OnBarometerFixedAction();
	public static event OnBarometerFixedAction OnBarometerFixed;
    #endregion

    
    //TODO: Insert Proper connectivity to the Action Sequencer
	//TODO: Handle gesture allowance
    void OnEnable()
    {
		ActionSequencerManager.OnBarometerNode += TriggerBreakBarometer;
		ActionSequencerItem.OnFailed += Reset;
    }
    void OnDisable()
    {
		ActionSequencerManager.OnBarometerNode -= TriggerBreakBarometer;
		ActionSequencerItem.OnFailed -= Reset;
    }
	
	#region Monobehaviour Functions
	void Awake () 
	{
        GSS = transform.GetComponentInChildren<GenericSoundScript>();
		InitializeBarometers();
	}
	
	void Update () 
	{
		RotateBarometers();
	}
	#endregion
	
	#region Class Methods
	private void InitializeBarometers()
	{
        for(int i = 0; i < _barometers.Length; i++)
        {
			_barometers[i].rotationSpeed = _normalRotationSpeed;
        }
		
		StartBarometers();
	}
	
	private void RotateBarometers()
	{
        for(int i = 0; i < _barometers.Length; i++)
        {
			if(_barometers[i].isRotating)
            _barometers[i].pointer.Rotate(Vector3.forward * -_barometers[i].rotationSpeed * Time.deltaTime, Space.Self);
        }
	}
	
	private void StartBarometers()
	{
        for(int i = 0; i < _barometers.Length; i++)
        {
			_barometers[i].isRotating = true;
        }
	}
	
	private void StopBarometers()
	{
        for(int i = 0; i < _barometers.Length; i++)
        {
			_barometers[i].isRotating = false;
        }
	}
	
	private void TriggerBreakBarometer()
	{
		GestureManager.OnDoubleTap += TriggerFixBarometer;
        var identifier = Random.Range(0,_barometers.Length);

        for(int i = 0; i < _barometers.Length; i++)
        {
            if(_barometers[i].isBroken == false)
            {
                BreakBarometer(identifier);
                break;
            }
            identifier++;

            if(identifier == _barometers.Length)
                identifier = 0;
        }
	}
	
	private void BreakBarometer(int i)
	{
		_barometers[i].rotationSpeed = _brokenRotationSpeed;
		_barometers[i].isBroken = true;
	}
	
	private void TriggerFixBarometer(GameObject go, Vector2 screenPos)
	{
        for(int i = 0; i < _barometers.Length; i++)
        {
			if(_barometers[i].isBroken == true && _barometers[i].barometer == go)
			{
				FixBarometer(i);
				GestureManager.OnDoubleTap -= TriggerFixBarometer;
				break;
			}
        }
	}
	
	private void FixBarometer(int i)
	{
        GSS.PlayClip(i);
		_barometers[i].rotationSpeed = _normalRotationSpeed;
		_barometers[i].isBroken = false;
		
		if(OnBarometerFixed != null)
			OnBarometerFixed();
	}
	
	private void Reset()
	{
		for(int i = 0; i < _barometers.Length; i++)
        {
			if(_barometers[i].isBroken == true)
			{
				_barometers[i].rotationSpeed = _normalRotationSpeed;
				_barometers[i].isBroken = false;
			}
        }
		GestureManager.OnDoubleTap -= TriggerFixBarometer;
	}
	#endregion
	
    #region SubClasses
    [System.Serializable]
    public class Barometers
    {
		public GameObject barometer;
        public Transform pointer;
		public float rotationSpeed;
        public bool isBroken = false;
		public bool isRotating = true;
    };
    #endregion
}
