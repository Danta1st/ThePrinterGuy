using UnityEngine;
using System.Collections;

public class Barometer : MonoBehaviour 
{
    #region Editor Publics
	[SerializeField] Barometers[] _barometers;
	[SerializeField] ParticleSystem _particleSystemStars;
	[SerializeField] ParticleSystem _particleSystemSmoke;
    #endregion

    #region Privates
	private float _normalRotationSpeed = 45;
	private float _brokenRotationSpeed = 720;
	private ParticleSystem _particleStars;
	private ParticleSystem _particleSmoke;
	private GameObject _dynamicObjects;
	
    #endregion

    #region Delegates & Events
	public delegate void OnBarometerFixedAction();
	public static event OnBarometerFixedAction OnBarometerFixed;
    #endregion

    
    //TODO: Insert Proper connectivity to the Action Sequencer
	//TODO: Handle gesture allowance
    void OnEnable()
    {
//		ActionSequencerManager.OnBarometerNode += TriggerBreakBarometer;
		ActionSequencerItem.OnFailed += Reset;
		
		BpmSequencer.OnBarometerNode += TriggerBreakBarometer;
    }
    void OnDisable()
    {
//		ActionSequencerManager.OnBarometerNode -= TriggerBreakBarometer;
		ActionSequencerItem.OnFailed -= Reset;
		
		BpmSequencer.OnBarometerNode -= TriggerBreakBarometer;
    }
	
	#region Monobehaviour Functions
	void Awake () 
	{
		_dynamicObjects = GameObject.Find("Dynamic Objects");
		if(_particleSystemSmoke != null)
		{
			_particleSmoke = (ParticleSystem)Instantiate(_particleSystemSmoke);
		}
		else
			Debug.Log("Smoke Particle not loaded for Barometer");
		if(_particleSystemStars != null)
		{
			_particleStars = (ParticleSystem)Instantiate(_particleSystemStars);
			_particleStars.renderer.material.shader = Shader.Find("Transparent/Diffuse");
		}
		else
			Debug.Log("Star Particle not loaded for Barometer");
		
		_particleSmoke.transform.parent = _dynamicObjects.transform;
		_particleStars.transform.parent = _dynamicObjects.transform;
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
	
	private void TriggerBreakBarometer(int itemNumber)
	{
		if(_barometers.Length < itemNumber + 1)
		{
			if(OnBarometerFixed != null)
				OnBarometerFixed();
			Debug.Log("ERROR: Number out of index!");
			return;
		}
			
		GestureManager.OnDoubleTap += TriggerFixBarometer;
		
		if(_barometers[itemNumber].isBroken == false)
        {
            BreakBarometer(itemNumber);
        }
		
        /* var identifier = Random.Range(0,_barometers.Length);

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
        }*/
	}
	
	private void BreakBarometer(int i)
	{
		foreach(Transform child in _barometers[i].barometer.transform)
		{
			if(child.name.Equals("ParticlePos") && _particleSmoke != null)
			{
				_particleSmoke.transform.position = child.position;
				_particleSmoke.transform.rotation = child.rotation;
				_particleSmoke.Play();
			}
		}
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
		_barometers[i].rotationSpeed = _normalRotationSpeed;
		_barometers[i].isBroken = false;
		if(_particleSmoke != null && _particleSmoke.isPlaying)
			_particleSmoke.Stop();
		foreach(Transform child in _barometers[i].barometer.transform)
		{
			if(child.name.Equals("ParticlePos") && _particleStars != null)
			{
				_particleStars.transform.position = child.position;
				_particleStars.transform.rotation = child.rotation;
				_particleStars.Play();
			}
		}
		
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
		if(_particleSmoke != null && _particleSmoke.isPlaying)
			_particleSmoke.Stop();
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
