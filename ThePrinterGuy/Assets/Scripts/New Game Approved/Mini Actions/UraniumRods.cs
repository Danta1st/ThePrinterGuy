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
	ParticleSystem _smokeParticle;
	ParticleSystem _starParticle;
	float _currentMoveY;
    private Dictionary<GameObject, bool> _rodsAndStates = new Dictionary<GameObject, bool>();
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
		_smokeParticle = (ParticleSystem)Instantiate(_particleSystemSmoke);
		_starParticle = (ParticleSystem)Instantiate(_particleSystemStars);
		_starParticle.renderer.material.shader = Shader.Find("Transparent/Diffuse");
        SetStates();
    }

    #region Class Methods
    //Initialise the states for each game object
    private void SetStates()
    {
        for(int i = 0; i < _rods.Length; i++)
        {
			_rods[i].startPos = _rods[i].rod.transform.localPosition.y;
			_rods[i].endPos = _rods[i].rod.transform.localPosition.y + 0.7f;
            _rodsAndStates.Add(_rods[i].rod, false);
        }
    }

    //Trigger a random rod, which is currently not up
    private void TriggerSpring()
    {
		GestureManager.OnTap += TriggerHammer;
        var identifier = Random.Range(0, _rods.Length);

        for(int i = 0; i < _rods.Length; i++)
        {
            var go = _rods[identifier].rod;
			
            if(_rodsAndStates[go] == false)
            {
                Spring(go, identifier);
                _rodsAndStates[go] = true;
				foreach(Transform child in go.transform)
				{
					if(child.name.Equals("ParticlePos"))
					{
						_smokeParticle.transform.position = child.position;
						_smokeParticle.transform.rotation = child.rotation;
						_smokeParticle.Play();
					}
				}
                break;
            }
            identifier++;

            if(identifier == _rods.Length)
                identifier = 0;
        }
    }

    private void Spring(GameObject go, int identifier)
    {
        iTween.MoveTo(go, iTween.Hash("y", _rods[identifier].endPos, "time", _outTime,
                                        "islocal", true, "easetype", _easeTypeOut));
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
		_smokeParticle.Stop();
		
		for(int i = 0; i < _rods.Length; i++)
		{
			if(_rods[i].rod == go)
			{
				iTween.MoveTo(go, iTween.Hash("y", _rods[i].startPos, "time", _inTime,
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
				if(child.name.Equals("ParticlePos"))
				{
					_starParticle.transform.position = child.position;
					_starParticle.transform.rotation = child.rotation;
					_starParticle.Emit(10);
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
		[HideInInspector]
        public float startPos;
		[HideInInspector]
		public float endPos;
    };
}
