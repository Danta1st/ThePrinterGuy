using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UraniumRods : MonoBehaviour
{
    #region Editor Publics
    [SerializeField]
    private iTween.EaseType _easeTypeOut = iTween.EaseType.easeOutExpo;
    [SerializeField]
    private iTween.EaseType _easeTypeIn = iTween.EaseType.easeOutBack;
    [SerializeField]
    private RodSet[] _rods;
	[SerializeField]
	private ParticleSystem _particleSystemStars;
	[SerializeField]
	private ParticleSystem _particleSystemSmoke;
    #endregion

    #region Privates
    private float _outTime = 0.5f;
    private float _inTime = 0.2f;
	private GameObject _currRod;
	private bool failed = false;
	ParticleSystem _particleSmoke;
	ParticleSystem _particleStars;
	private GameObject _dynamicObjects;
    private Dictionary<GameObject, bool> _rodsAndStates = new Dictionary<GameObject, bool>();
    private GenericSoundScript GSS;
    #endregion

    #region Delegates & Events
    public delegate void OnRodHammeredAction();
    public static event OnRodHammeredAction OnRodHammered;
    #endregion

    void OnEnable()
    {
		ActionSequencerManager.OnUraniumRodNode += TriggerSpring;
		ActionSequencerItem.OnFailed += Reset;
    }

    void OnDisable()
    {
		ActionSequencerManager.OnUraniumRodNode -= TriggerSpring;
		ActionSequencerItem.OnFailed -= Reset;
    }

    void Awake()
    {

		_dynamicObjects = GameObject.Find("Dynamic Objects");
		if(_particleSystemSmoke != null)
		{
			_particleSmoke = (ParticleSystem)Instantiate(_particleSystemSmoke);
		}
		else
			Debug.Log("Smoke Particle not loaded for UranRods");
		if(_particleSystemStars != null)
		{
			_particleStars = (ParticleSystem)Instantiate(_particleSystemStars);
			_particleStars.renderer.material.shader = Shader.Find("Transparent/Diffuse");
		}
		else
			Debug.Log("Star Particle not loaded for UranRods");
		
		_particleSmoke.transform.parent = _dynamicObjects.transform;
		_particleStars.transform.parent = _dynamicObjects.transform;

        GSS = transform.GetComponentInChildren<GenericSoundScript>();

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

    //Trigger a random rod, which is currently not up
    private void TriggerSpring(int itemNumber)
    {
		if(_rods.Length < itemNumber + 1)
		{
			if(OnRodHammered != null)
				OnRodHammered();
			Debug.Log("ERROR RODS: Number out of index!");
			return;
		}
		GestureManager.OnTap += TriggerHammer;
        
		var go = _rods[itemNumber].rod;
		//if(_rodsAndStates[go] == false) // QUICKFIX: Enables ability to choose same rod several times in a row
        {
            GSS.PlayClip(itemNumber);
            Spring(go, itemNumber);

            _rodsAndStates[go] = true;
			foreach(Transform child in go.transform)
			{
				if(child.name.Equals("ParticlePos") && _particleSmoke != null)
				{
					_particleSmoke.transform.position = child.position;
					_particleSmoke.transform.rotation = child.rotation;
					_particleSmoke.Play();
				}
			}
        }
		
		/*var identifier = Random.Range(0, _rods.Length);

        for(int i = 0; i < _rods.Length; i++)
        {
            var go = _rods[identifier].rod;
			
            if(_rodsAndStates[go] == false)
            {
                GSS.PlayClip(i);
                Spring(go, identifier);

                _rodsAndStates[go] = true;
				foreach(Transform child in go.transform)
				{
					if(child.name.Equals("ParticlePos") && _particleSmoke != null)
					{
						_particleSmoke.transform.position = child.position;
						_particleSmoke.transform.rotation = child.rotation;
						_particleSmoke.Play();
					}
				}
                break;
            }
            identifier++;

            if(identifier == _rods.Length)
                identifier = 0;
        }*/
    }

    private void Spring(GameObject go, int identifier)
    {
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

                GSS.PlayClip(3);

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
		if(_particleSmoke != null && _particleSmoke.isPlaying)
			_particleSmoke.Stop();
		
		for(int i = 0; i < _rods.Length; i++)
		{
			if(_rods[i].rod == go)
			{
				iTween.MoveTo(go, iTween.Hash("position", _rods[i].startPos, "time", _inTime,
                             "islocal", true, "easetype", _easeTypeIn, "oncomplete", "HammerComplete", "oncompletetarget", gameObject, "oncompleteparams", go));
			}
		}
    }
	
	private void HammerComplete(GameObject go)
	{
		if(!failed)
		{
			foreach(Transform child in go.transform)
			{
				if(child.name.Equals("ParticlePos") && _particleStars != null)
				{
					_particleStars.transform.position = child.position;
					_particleStars.transform.rotation = child.rotation;
					_particleStars.Play();
				}
			}
		}
		failed = false;
		_rodsAndStates[go] = false;
	}
    //Reset all the rods and disables GestureManager (Called on Failed)
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
    #endregion
	
	[System.Serializable]
    public class RodSet
    {
        public GameObject rod;
		public GameObject rodMoveTo;
		[HideInInspector]
        public Vector3 startPos;
    };
}
