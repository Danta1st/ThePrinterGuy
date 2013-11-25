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
    [SerializeField] private List<PaperLightSet> _paperlightset;

	[SerializeField] private ParticleSystem _particleSystemStars;
	[SerializeField] private ParticleSystem _particleSystemSmoke;
	[SerializeField] private ParticleSystem _particleSystemIncinerate;


//    [SerializeField] private AudioClip clipUp;
//    [SerializeField] private AudioClip clipDown;

    #endregion

    #region Privates
	//Gate Variables
    private bool _isGateOpen    = true;
    private float _openTime     = 0.4f;
    private float _closeTime    = 0.4f;
	
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
	private ParticleSystem _particleFlames;
	private float _burnTime 	= 0.5f;
	
	//Gate positions
	private Vector3 _beginPosition;
	private Vector3 _openPosition;
    #endregion


    #region Delegates & Events
    public delegate void OnPaperInsertedAction();
    public static event OnPaperInsertedAction OnCorrectPaperInserted;
    //public static event OnPaperInsertedAction OnIncorrectPaperInserted;
    #endregion

	
    void OnEnable()
    {
		StartGate();
		
		BpmSequencer.OnPaperNode += TriggerLight;
		BpmSequencer.OnPaperNode += EnablePaper;
		ActionSequencerItem.OnFailed += Reset;
    }
    void OnDisable()
    {
		StopGate();
		
		BpmSequencer.OnPaperNode -= TriggerLight;
		BpmSequencer.OnPaperNode -= EnablePaper;
		ActionSequencerItem.OnFailed -= Reset;
    }

    #region Monobehaviour Functions
	void Awake()
	{
		if(GameObject.Find("BPM_Manager") == null)
			Debug.LogError("Level Dependency object 'BPM_Manager' couldn't be found");
		
		//Set gate positions;
		_beginPosition = _gate.transform.position;
		_openPosition = _gate.transform.position + Vector3.up * 3;
		
		//Get dynamic objects to instantiate objects in
		_dynamicObjects = GameObject.Find("Dynamic Objects");
		if(_dynamicObjects == null)
			Debug.LogError("Level Dependency object 'Dynamic Objects' couldn't be found");
		
		//Check for particle systems.
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
		if(_particleSystemIncinerate != null)
		{
			_particleFlames = (ParticleSystem)Instantiate (_particleSystemIncinerate);
		}
		_particleSmoke.transform.parent = _dynamicObjects.transform;
		_particleStars.transform.parent = _dynamicObjects.transform;

		InitializeLights();
		DisablePaper();
	}
    #endregion

    #region Class Methods
    //Gate Methods
    private void StartGate()
    {
		BeatController.OnBeat4th1 += OpenGate;
		BeatController.OnBeat4th1 += SoundManager.Effect_PaperTray_MoveUp;
		BeatController.OnBeat4th4 += CloseGate;
		BeatController.OnBeat4th4 += SoundManager.Effect_PaperTray_MoveDown;
    }

    private void StopGate()
    {
		BeatController.OnBeat4th1 -= OpenGate;
		BeatController.OnBeat4th1 -= SoundManager.Effect_PaperTray_MoveUp;
		BeatController.OnBeat4th4 -= CloseGate;
		BeatController.OnBeat4th4 -= SoundManager.Effect_PaperTray_MoveDown;
    }

    private void OpenGate()
    {
        if(!_isGateOpen)
        {
			iTween.MoveTo(_gate, iTween.Hash("position", _openPosition, "time", _openTime, "easetype", _easeTypeOpen));
			
            _isGateOpen = true;
        }
    }

    private void CloseGate()
    {
        if(_isGateOpen)
        {
			iTween.MoveTo(_gate, iTween.Hash("position", _beginPosition, "time", _closeTime, "easetype", _easeTypeClose));
			
            _isGateOpen = false;
        }
    }
	
	
    //Light Methods
    private void InitializeLights()
    {
        for(int i = 0; i < _paperlightset.Count; i++)
        {
            _paperlightset[i].light.renderer.material.mainTexture = _paperlightset[i].off;
        }
    }

    private void TriggerLight(int itemNumber)
    {
		if(_paperlightset.Count < itemNumber)
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
            //GSS.PlayClip(Random.Range(3, 4));
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
        for(int i = 0; i < _paperlightset.Count; i++)
        {
            TurnOffLight(i);
        }
    }
	
	//Paper Methods
	//TODO: Paper animations
    private void DisablePaper()
    {
        for(int i = 0; i < _paperlightset.Count; i++)
        {
            _paperlightset[i].paper.SetActive(false);
			_paperlightset[i].paperToSlide.SetActive(false);
        }
    }

    private void EnablePaper(int temp)
    {
		//GestureManager.OnSwipeUp += TriggerSlide;
		GestureManager.OnTap += TriggerSlide;
        for(int i = 0; i < _paperlightset.Count; i++)
        {
            _paperlightset[i].paper.SetActive(true);
			_paperlightset[i].paperToSlide.SetActive(true);
        }
    }
	
	private void TriggerSlide(GameObject go, Vector2 screenPosition)
	{
		
		if(go != null)
		{
			int j = 0;
			PaperLightSet paper;
			int count = _paperlightset.Count;
			for(int i = 0; i < count; i++)
			{
				paper = _paperlightset[j];
				if(paper.paper == null)
				{
					_paperlightset.Remove(paper);
					_paperlightset.TrimExcess();
					continue;
				}
				j++;
			}
			
	        for(int i = 0; i < _paperlightset.Count; i++)
	        {
				//Succesfull Slide
	            if(_paperlightset[i].isOn && _paperlightset[i].paper.transform == go.transform)
				{
					playSlideSound(i);
					SlidePaper(i, true);
					break;
				}
				//Unsuccesfull Slide
				else if(!_paperlightset[i].isOn && _paperlightset[i].paper.transform == go.transform)
				{
                    SoundManager.Effect_PaperTray_WrongSwipe();
					SlidePaper(i, false);
					break;
				}
	        }
		}
	}

    private void SlidePaper(int i, bool colorMatch)
    {
		if(_IsSlideLocked == false)
		{
			if(_isGateOpen)
			{
				TurnOffLight(i);
				_IsSlideLocked = true;
				
				var paper = (GameObject) Instantiate(_paperlightset[i].paperToSlide, _paperlightset[i].paper.transform.position, _paperlightset[i].paper.transform.rotation);
				paper.transform.parent = _dynamicObjects.transform;
				_tempPaper.Add(paper);
				
				
				if(colorMatch) 
				{
					Reset();
					iTween.MoveTo(paper, iTween.Hash("position", _target.transform.position, "time", _slideTime, "easetype", _easeTypeSlide, 
													"oncomplete", "DestroyPaper", "oncompleteparams", paper, "oncompletetarget", gameObject));
				}
				else
				{
					Hashtable iTweenParams = new Hashtable();
					iTweenParams.Add("go", paper);
					iTweenParams.Add("index", i);
					iTween.MoveTo(paper, iTween.Hash("position", _target.transform.position, "time", _slideTime, "easetype", _easeTypeSlide, 
													"oncomplete", "IncineratePaper", "oncompleteparams", iTweenParams, "oncompletetarget", gameObject));
				}
				
				foreach(Transform child in gameObject.transform)
				{
					if(child.name.Equals("ParticlePos") && _particleStars != null)
					{
						_particleStars.transform.position = child.position;
						_particleStars.transform.rotation = child.rotation;
						_particleStars.Play();
					}
				}
				if(_particleSmoke != null && _particleSmoke.isPlaying && colorMatch)
					_particleSmoke.Stop();
				
				if(OnCorrectPaperInserted != null && colorMatch)
					OnCorrectPaperInserted();
				
			}
			else
			{
				_IsSlideLocked = true;
				
				var paper = (GameObject) Instantiate(_paperlightset[i].paperToSlide, _paperlightset[i].paper.transform.position, _paperlightset[i].paper.transform.rotation);
				paper.transform.parent = _dynamicObjects.transform;
				_tempPaper.Add(paper);
				
				Hashtable iTweenParams = new Hashtable();
				iTweenParams.Add("go", paper);
				iTweenParams.Add("index", i);
				iTween.MoveTo(paper, iTween.Hash("position", _gate.gameObject.transform.position, "time", _slideTime, "easetype", _easeTypeSlide, 
													"oncomplete", "IncineratePaper", "oncompleteparams", iTweenParams, "oncompletetarget", gameObject));			
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
	
	IEnumerator IncineratePaper(object obj)
	{
		Hashtable ht = (Hashtable)obj;
		GameObject go = (GameObject)ht["go"];
		//int index = (int)ht["index"];
		_particleFlames.transform.position = go.transform.position;
		if(_particleFlames != null && _particleFlames.isPlaying)
			_particleFlames.Stop();
		_particleFlames.Play ();
		yield return new WaitForSeconds(_burnTime);
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
	
	private void playSlideSound(int i)
	{
		//Play Sound
		switch(i)
		{
		case 0:
			SoundManager.Effect_PaperTray_Swipe1();
			break;
		case 1:
			SoundManager.Effect_PaperTray_Swipe2();
			break;
		case 2:
			SoundManager.Effect_PaperTray_Swipe3();
			break;
		case 3:
			SoundManager.Effect_PaperTray_Swipe4();
			break;
		default:
			Debug.LogWarning("Incorrect itemNumber for paperSlide received. No sound will play on sliding");
			break;
		}
		
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
		public GameObject paperToSlide;
    };
    #endregion
}
