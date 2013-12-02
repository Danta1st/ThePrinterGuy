using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UraniumRods : MonoBehaviour
{
    #region Editor Publics
    [SerializeField] private iTween.EaseType _easeTypeOut = iTween.EaseType.easeOutExpo;
    [SerializeField] private iTween.EaseType _easeTypeIn = iTween.EaseType.easeOutBack;
    [SerializeField] private RodSet[] _rods;
	//Particle Systems
	[SerializeField] private Particles _particles;
    #endregion

    #region Privates
	//Move Variables
    private float _outTime = 0.2f;
    private float _inTime = 0.2f;
	
	private bool failed = false;
	private GameObject _dynamicObjects;
    private Dictionary<GameObject, bool> _rodsAndStates = new Dictionary<GameObject, bool>();
    #endregion

    #region Delegates & Events
    public delegate void OnRodHammeredAction();
    public static event OnRodHammeredAction OnRodHammered;
    #endregion

    void OnEnable()
    {
		BpmSequencer.OnUraniumRodNode += TriggerSpring;
		BpmSequencerItem.OnFailed += Reset;
    }
    void OnDisable()
    {
		BpmSequencer.OnUraniumRodNode -= TriggerSpring;
		BpmSequencerItem.OnFailed -= Reset;
    }	
	void OnDestroy()
	{
		BpmSequencer.OnUraniumRodNode -= TriggerSpring;
		BpmSequencerItem.OnFailed -= Reset;		
	}

    void Awake()
    {
		_dynamicObjects = GameObject.Find("Dynamic Objects");
		
		//Check if _particles if empty, and throw warning if true
		if(_particles._hammerBegan == null)
			Debug.LogWarning(gameObject.name+" reported that a particle prefab in '_particles' is empty");
		if(_particles._hammerComplete == null)
			Debug.LogWarning(gameObject.name+" reported that a particle prefab in '_particles' is empty");
		if(_particles._spring == null)
			Debug.LogWarning(gameObject.name+" reported that a particle prefab in '_particles' is empty");
		
        SetStates();
    }

    #region Class Methods
    //Initialise the states for each game object
    private void SetStates()
    {
        for(int i = 0; i < _rods.Length; i++)
        {
			_rods[i].startPos = _rods[i].rod.transform.localPosition;
            _rodsAndStates.Add(_rods[i].rod, false);
        }
    }
	
	//ReTrigger - Called from GUIGameCamera when player hits a uranium rod to early.
	public void ReTriggerSpring()
	{
		GestureManager.OnTap += TriggerHammer;
		
		var go = _rods[_currentSprung].rod;
		
        StartCoroutine(Spring(go, _currentSprung));

        _rodsAndStates[go] = true;
	
		InstantiateParticles(_particles._spring, go);			
	}
	
	//Triger the a specific rod based on itemNumber
	private int _currentSprung = 0;
    private void TriggerSpring(int itemNumber)
    {
		_currentSprung = itemNumber;
		//TODO: Please mark what this code does it you implement it again!
//		if(_rods.Length < itemNumber + 1)
//		{
//			if(OnRodHammered != null)
//				OnRodHammered();
//			return;
//		}
//		else			
//			Debug.LogWarning("ERROR RODS: itemNumber out of index!");
		
		//Subscribe hammering to OnTap
		GestureManager.OnTap += TriggerHammer;
        
		var go = _rods[itemNumber].rod;
		
        StartCoroutine(Spring(go, itemNumber));

        _rodsAndStates[go] = true;
	
		InstantiateParticles(_particles._spring, go);		
    }

    private IEnumerator Spring(GameObject go, int identifier)
    {
		//Play sound
		SoundManager.Effect_UraniumRods_Hammer();
		//Move rod
		if(iTween.Count (go) > 0)
			yield return new WaitForSeconds(_inTime);
        
		iTween.MoveTo(go, iTween.Hash("position", _rods[identifier].rodMoveTo.transform.position, "time", _outTime,
                                        "easetype", _easeTypeOut));
    }

    //Hammer the rod if it is currently up
    private void TriggerHammer(GameObject go, Vector2 screenPos)
    {
		int i = 0;
        foreach(KeyValuePair<GameObject, bool> pair in _rodsAndStates)
        {
            if(pair.Key == go && pair.Value == true)
            {
				failed = false;

                Hammer(go);
				GestureManager.OnTap -= TriggerHammer;
                if(OnRodHammered != null)
					OnRodHammered();
				i++;
                break;
            }
        }
    }

    private void Hammer(GameObject go)
    {
		if(go == null)
			return;
		
		GestureManager.OnTap -= TriggerHammer;
		
		for(int i = 0; i < _rods.Length; i++)
		{
			if(_rods[i].rod == go)
			{
				_rodsAndStates[go] = false;
				//Instantiate Particles
				InstantiateParticles(_particles._hammerBegan, go);
				//Play sound based on itemNumber
				PlayHammerSound(i);
				//Move rod back into place
				iTween.MoveTo(go, iTween.Hash("position", _rods[i].startPos, "time", _inTime,
                             "islocal", true, "easetype", _easeTypeIn, "oncomplete", "HammerComplete", "oncompletetarget", gameObject, "oncompleteparams", go));
			}
		}
    }
	
	private void HammerComplete(GameObject go)
	{
		if(!failed)
		{
			//Instantiate particles
			InstantiateParticles(_particles._hammerComplete, go);
			
			//TODO: Disable Smoke effect
		}
		failed = false;
	}
	
    //Reset all the rods and disables GestureManager (Called on Failed == true)
    private void Reset()
    {
		GameObject go;
		for(int i = 0; i < _rods.Length; i++)
        {
			go = _rods[i].rod;
            if(_rodsAndStates[go] == true)
            {
				failed = true;
                Hammer(go);
            }
        }
    }
	
	//Method for selecting the right hammer sound
	private void PlayHammerSound(int i)
	{
		//Play Sound
		switch(i)
		{
		case 0:
			SoundManager.Effect_UraniumRods_Popup1();
			break;
		case 1:
			SoundManager.Effect_UraniumRods_Popup2();
			break;
		case 2:
			SoundManager.Effect_UraniumRods_Popup3();
			break;
		case 3:
			SoundManager.Effect_UraniumRods_Popup4();
			break;
		default:
			Debug.LogWarning("Incorrect itemNumber for uranium received. No sound will play on hammering");
			break;
		}		
	}	
	
	//Method for instantiating particles
	private void InstantiateParticles(GameObject particles, GameObject posRotGO)
	{
		if(particles != null)
		{
			foreach(Transform child in posRotGO.transform)
			{
				if(child.name.Equals("ParticlePos"))
				{
					//Instantiate Particle prefab. Rotation solution is a HACK
					GameObject tempParticles = (GameObject) Instantiate(particles, child.position, Quaternion.FromToRotation(particles.transform.up, -child.up));
					//Child to DynamicObjects
					tempParticles.transform.parent = _dynamicObjects.transform;
					Debug.Log(gameObject.name+" instantiating particles");
				}
			}	
		}
	}
    #endregion
	
	[System.Serializable]
    public class RodSet
    {
        public GameObject rod;
		public GameObject rodMoveTo;
		[HideInInspector]
        public Vector3 startPos;
    };
	
	[System.Serializable]
	public class Particles
	{
		public GameObject _spring;
		public GameObject _hammerBegan;
		public GameObject _hammerComplete;		
	}
}
