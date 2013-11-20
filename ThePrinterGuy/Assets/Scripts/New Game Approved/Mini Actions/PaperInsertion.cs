using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PaperInsertion : MonoBehaviour
{
    #region Editor Publics
    [SerializeField] private GameObject _gate;
    [SerializeField] private iTween.EaseType _easeTypeOpen  = iTween.EaseType.easeOutCirc;
    [SerializeField] private iTween.EaseType _easeTypeClose = iTween.EaseType.easeOutBounce;
	[SerializeField] private GameObject _target;
    [SerializeField] private PaperLightSet[] _paperlightset;

	[SerializeField] private ParticleSystem _particleSystemStars;
	[SerializeField] private ParticleSystem _particleSystemSmoke;

//    [SerializeField] private AudioClip clipUp;
//    [SerializeField] private AudioClip clipDown;

    #endregion

    #region Privates
	//Gate Variables
    private bool _isGateOpen    = true;
    private bool _isGateAllowedToRun = false;
    private float _openTime     = 0.5f;
    private float _closeTime    = 0.5f;
    private float _waitTime     = 0.2f;
	
	//Paper Slide variables
	private iTween.EaseType _easeTypeSlide = iTween.EaseType.easeOutExpo;
	private bool _IsSlideLocked		= false;
	private float _slideTime		= 0.4f;
	private float _slideWait		= 0.2f;
	private ArrayList _tempPaper = new ArrayList();
	
	//Whatever
	private GameObject _dynamicObjects;

	private ParticleSystem _particleSmoke;
	private ParticleSystem _particleStars;    

    private GenericSoundScript GSS;
    #endregion


    #region Delegates & Events
    public delegate void OnPaperInsertedAction();
    public static event OnPaperInsertedAction OnCorrectPaperInserted;
    //public static event OnPaperInsertedAction OnIncorrectPaperInserted;
    #endregion

    //TODO: Insert Proper connectivity to the Action Sequencer
	//TODO: Handle gesture allowance
    void OnEnable()
    {
		ActionSequencerManager.OnPaperNode += TriggerLight;
		ActionSequencerManager.OnPaperNode += EnablePaper;
		ActionSequencerItem.OnFailed += Reset;
		
        StartGate();
    }
    void OnDisable()
    {
		ActionSequencerManager.OnPaperNode -= TriggerLight;
		ActionSequencerManager.OnPaperNode -= EnablePaper;
		ActionSequencerItem.OnFailed -= Reset;
		
        StopGate();
    }

    #region Monobehaviour Functions
	void Awake()
	{

		_dynamicObjects = GameObject.Find("Dynamic Objects");
		if(_particleSystemSmoke != null)
		{
			_particleSmoke = (ParticleSystem)Instantiate(_particleSystemSmoke);
		}
		else
			Debug.Log("Smoke Particle not loaded for Paper");
		if(_particleSystemStars != null)
		{
			_particleStars = (ParticleSystem)Instantiate(_particleSystemStars);
			_particleStars.renderer.material.shader = Shader.Find("Transparent/Diffuse");
		}
		else
			Debug.Log("Star Particle not loaded for Paper");
		
		_particleSmoke.transform.parent = _dynamicObjects.transform;
		_particleStars.transform.parent = _dynamicObjects.transform;

        GSS = transform.GetComponentInChildren<GenericSoundScript>();
		_dynamicObjects = GameObject.Find("Dynamic Objects");	

		InitializeLights();
		DisablePaper();
	}
    #endregion

    #region Class Methods
    //Gate Methods
    private void StartGate()
    {
        if(!_isGateAllowedToRun)
        {
            _isGateAllowedToRun = true;

            if(!_isGateOpen)
                OpenGate();
            else
                CloseGate();
        }
    }

    private void StopGate()
    {
        _isGateAllowedToRun = false;
    }

    private void OpenGate()
    {
        if(!_isGateOpen)
        {
            GSS.PlayClip(0);
            iTween.MoveTo(_gate,iTween.Hash("y", _gate.transform.localPosition.y + 3, "time", _openTime,
                                            "islocal", true, "easetype", _easeTypeOpen, "oncomplete", "NextAnimation",
                                            "oncompletetarget", gameObject));
            _isGateOpen = true;
//            gameObject.audio.PlayOneShot(clipUp);
        }
    }

    private void CloseGate()
    {
        if(_isGateOpen)
        {
            GSS.PlayClip(1);
            iTween.MoveTo(_gate,iTween.Hash("y", _gate.transform.localPosition.y - 3, "time", _closeTime,
                                            "islocal", true, "easetype", _easeTypeClose, "oncomplete", "NextAnimation",
                                            "oncompletetarget", gameObject));
            _isGateOpen = false;
//            gameObject.audio.PlayOneShot(clipDown);
        }
    }

    private void NextAnimation()
    {
        if(_isGateAllowedToRun)
        {
            if(_isGateOpen)
                Invoke("CloseGate",_waitTime);
            else
                Invoke("OpenGate",_waitTime);
        }
    }


    //Light Methods
    private void InitializeLights()
    {
        for(int i = 0; i < _paperlightset.Length; i++)
        {
            _paperlightset[i].light.renderer.material.mainTexture = _paperlightset[i].off;
        }
    }

    private void TriggerLight(int itemNumber)
    {
		if(_paperlightset.Length < itemNumber + 1)
		{
			if(OnCorrectPaperInserted != null)
				OnCorrectPaperInserted();
			Debug.Log("ERROR PAPER: Number out of index!");
			return;
		}
		foreach(Transform child in gameObject.transform)
		{
			if(child.name.Equals("ParticlePos") && _particleSmoke != null)
			{
				_particleSmoke.transform.position = child.position;
				_particleSmoke.transform.rotation = child.rotation;
				_particleSmoke.Play();
			}
		}
		if(_paperlightset[itemNumber].isOn == false)
        {
            GSS.PlayClip(Random.Range(3, 4));
            TurnOnLight(itemNumber);
        }
        
		//Method for randomisation
		
		/*var identifier = Random.Range(0,_paperlightset.Length);

        for(int i = 0; i < _paperlightset.Length; i++)
        {
            if(_paperlightset[identifier].isOn == false)
            {
                GSS.PlayClip(Random.Range(3, 4));
                TurnOnLight(identifier);
                break;
            }
            identifier++;

            if(identifier == _paperlightset.Length)
                identifier = 0;
        }*/
    }

    private void TurnOnLight(int i)
    {
        if(_paperlightset[i].isOn == false)
        {
            _paperlightset[i].light.renderer.material.mainTexture = _paperlightset[i].on;
            _paperlightset[i].isOn = true;
        }
    }

    private void TurnOffLight(int i)
    {
        if(_paperlightset[i].isOn == true)
        {
            _paperlightset[i].light.renderer.material.mainTexture = _paperlightset[i].off;
            _paperlightset[i].isOn = false;
        }
    }

    private void TurnOfAllLights()
    {
        for(int i = 0; i < _paperlightset.Length; i++)
        {
            TurnOffLight(i);
        }
    }
	
	//Paper Methods
	//TODO: Paper animations
    private void DisablePaper()
    {
        for(int i = 0; i < _paperlightset.Length; i++)
        {
            _paperlightset[i].paper.SetActive(false);
        }
    }

    private void EnablePaper(int temp)
    {
		//GestureManager.OnSwipeUp += TriggerSlide;
		GestureManager.OnTap += TriggerSlide;
        for(int i = 0; i < _paperlightset.Length; i++)
        {
            _paperlightset[i].paper.SetActive(true);
        }
    }
	
	private void TriggerSlide(GameObject go, Vector2 screenPosition)
	{
		if(go != null)
		{
	        for(int i = 0; i < _paperlightset.Length; i++)
	        {
	            if(_paperlightset[i].isOn && _paperlightset[i].paper.transform == go.transform.parent)
				{
                    GSS.PlayClip(Random.Range(5, 8));
					SlidePaper(i);
					break;
				}
	        }
		}
	}

    private void SlidePaper(int i)
    {
		if(_IsSlideLocked == false)
		{
			if(_isGateOpen)
			{
				TurnOffLight(i);
				_IsSlideLocked = true;
				
				var paper = (GameObject) Instantiate(_paperlightset[i].paper, _paperlightset[i].paper.transform.position, _paperlightset[i].paper.transform.rotation);
				paper.transform.parent = _dynamicObjects.transform;
				_tempPaper.Add(paper);
				
				Reset();
				
				iTween.MoveTo(paper, iTween.Hash("position", _target.transform.position, "time", _slideTime, "easetype", _easeTypeSlide, 
													"oncomplete", "DestroyPaper", "oncompleteparams", paper, "oncompletetarget", gameObject));
				
				foreach(Transform child in gameObject.transform)
				{
					if(child.name.Equals("ParticlePos") && _particleStars != null)
					{
						_particleStars.transform.position = child.position;
						_particleStars.transform.rotation = child.rotation;
						_particleStars.Play();
					}
				}
				if(_particleSmoke != null && _particleSmoke.isPlaying)
					_particleSmoke.Stop();
				
				if(OnCorrectPaperInserted != null)
					OnCorrectPaperInserted();
				
			}
			else
			{
				_IsSlideLocked = true;
				
				var paper = (GameObject) Instantiate(_paperlightset[i].paper);
				paper.transform.parent = _dynamicObjects.transform;
				_tempPaper.Add(paper);
				
				iTween.MoveTo(paper, iTween.Hash("position", _gate.transform.position, "time", _slideTime, "easetype", _easeTypeSlide, 
													"oncomplete", "DestroyPaper", "oncompleteparams", paper, "oncompletetarget", gameObject));			
			}
		}
    }
	
	private void TriggerDestroyPaper(GameObject go)
	{
		StartCoroutine(DestroyPaper(go));
	}
	
	IEnumerator DestroyPaper(GameObject go)
	{
		yield return new WaitForSeconds(_slideWait);
		_tempPaper.Remove(go);
		Destroy(go);
		_IsSlideLocked = false;
	}
	
	public void Reset()
	{
		if(_particleSmoke != null && _particleSmoke.isPlaying)
			_particleSmoke.Stop();
		//GestureManager.OnSwipeUp -= TriggerSlide;
		GestureManager.OnTap += TriggerSlide;
		DisablePaper();
		TurnOfAllLights();
	}
    #endregion

    #region SubClasses
    [System.Serializable]
    public class PaperLightSet
    {
        public Transform light;
        public Texture on;
        public Texture off;
        public GameObject paper;
        public bool isOn = false;
    };
    #endregion
}
