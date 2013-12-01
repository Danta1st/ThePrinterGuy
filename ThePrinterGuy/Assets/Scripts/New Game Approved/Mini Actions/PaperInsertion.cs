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
	//Particles - New
	[SerializeField] private Particles _particles;
//    [SerializeField] private GameObject _paperParticles;
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
	
	//Whatever
	private GameObject _dynamicObjects;
    private GameObject _conveyorBelt;

	//Gate positions
	private Vector3 _beginPosition;
	private Vector3 _openPosition;
	
	//Paper control statement
	private bool _isPaperEnabled = false;
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
		BpmSequencerItem.OnFailed += Reset;
    }
    void OnDisable()
    {
		StopGate();		
		BpmSequencer.OnPaperNode -= TriggerLight;
		BpmSequencer.OnPaperNode -= EnablePaper;
        GestureManager.OnTap -= TriggerSlide;
		BpmSequencerItem.OnFailed -= Reset;
    }	
	void OnDestroy()
	{
		StopGate();
		BpmSequencer.OnPaperNode -= TriggerLight;
		BpmSequencer.OnPaperNode -= EnablePaper;
		GestureManager.OnTap -= TriggerSlide;
		BpmSequencerItem.OnFailed -= Reset;
		UnsubscribePaperPunch();
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
		
		//Check if _particles if empty, and throw warning if true
		if(_particles.complete == null)
			Debug.LogWarning(gameObject.name+" reported that a particle prefab in '_particles' is empty");
		if(_particles.failed == null)
			Debug.LogWarning(gameObject.name+" reported that a particle prefab in '_particles' is empty");
		if(_particles.enablePaper == null)
			Debug.LogWarning(gameObject.name+" reported that a particle prefab in '_particles' is empty");
		if(_particles.disablePaper == null)
			Debug.LogWarning(gameObject.name+" reported that a particle prefab in '_particles' is empty");

		InitializeLights();
		DisablePaper();

        _conveyorBelt = transform.FindChild("BB-9000_Paper").FindChild("convoyeBelt").gameObject;
	}
    #endregion

    void Update()
    {
        RollConvoreBelt();
    }

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
//		if(_paperlightset.Count < itemNumber)
//		{
//			if(OnCorrectPaperInserted != null)
//				OnCorrectPaperInserted();
//			Debug.Log("ERROR PAPER: Number out of index!");
//			return;
//		}
		
		if(_paperlightset[itemNumber].isOn == false)
        {
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
			SubscribePaperPunch(i);
            _paperlightset[i].light.renderer.material.mainTexture = _paperlightset[i].on;
            _paperlightset[i].isOn = true;
//            _paperParticles.transform.position = _paperlightset[i].paperToSlide.transform.position;
//            _paperParticles.SetActive(true);
        }
    }
	
	private int _tempPunch = 0;
	private float _punchTime = 0.45f;
	private void SubscribePaperPunch(int itemNumber)
	{
		_tempPunch = itemNumber;
		
		BeatController.OnBeat4th1 += PunchPaper;
		BeatController.OnBeat4th2 += PunchPaper;
		BeatController.OnBeat4th3 += PunchPaper;
		BeatController.OnBeat4th4 += PunchPaper;
	}
	private void UnsubscribePaperPunch()
	{		
		BeatController.OnBeat4th1 -= PunchPaper;
		BeatController.OnBeat4th2 -= PunchPaper;
		BeatController.OnBeat4th3 -= PunchPaper;
		BeatController.OnBeat4th4 -= PunchPaper;
	}
		
	private void PunchPaper()
	{
		if(_tempPunch != null)
			iTween.PunchScale(_paperlightset[_tempPunch].paper, new Vector3(0.1f, 0.1f, 0.1f), _punchTime);
	}
	
    private void TurnOffLight(int i)
    {
        if(_paperlightset[i].isOn == true)
        {
			UnsubscribePaperPunch();
            _paperlightset[i].light.renderer.material.mainTexture = _paperlightset[i].off;
            _paperlightset[i].isOn = false;
//            _paperParticles.SetActive(false);
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
		if(_isPaperEnabled)
		{
			GestureManager.OnTap -= TriggerSlide;
	        for(int i = 0; i < _paperlightset.Count; i++)
	        {
				//Spawn particles
				InstantiateParticles(_particles.disablePaper, _paperlightset[i].paper);
				//Disable objects
	            _paperlightset[i].paper.SetActive(false);
				_paperlightset[i].paperToSlide.SetActive(false);
	        }
            _isPaperEnabled = false;
		}
    }

    private void EnablePaper(int notUsed)
    {
		if(!_isPaperEnabled)
		{
			GestureManager.OnTap += TriggerSlide;
	        for(int i = 0; i < _paperlightset.Count; i++)
	        {
				//Spawn particles
				InstantiateParticles(_particles.enablePaper, _paperlightset[i].paper);
				//Enable objects
	            _paperlightset[i].paper.SetActive(true);
				_paperlightset[i].paperToSlide.SetActive(true);
	        }
            _isPaperEnabled = true;
		}
    }
	
	private void TriggerSlide(GameObject go, Vector2 screenPosition)
	{
		
		if(go != null)
		{
			//Please explain why this code is necessary
//			int j = 0;
//			PaperLightSet paper;
//			int count = _paperlightset.Count;
//			for(int i = 0; i < count; i++)
//			{
//				paper = _paperlightset[j];
//				if(paper.paper == null)
//				{
//					_paperlightset.Remove(paper);
//					_paperlightset.TrimExcess();
//					continue;
//				}
//				j++;
//			}
			
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
				
				//Block the player from sliding
				_IsSlideLocked = true;
				
				var paper = (GameObject) Instantiate(_paperlightset[i].paperToSlide, _paperlightset[i].paper.transform.position, _paperlightset[i].paper.transform.rotation);
				paper.transform.parent = _dynamicObjects.transform;
				
				//Enable Paper Trail or throw warning
				if(paper.GetComponentInChildren<TrailRenderer>() != null)
					paper.GetComponentInChildren<TrailRenderer>().enabled = true;
				else
					Debug.LogWarning(gameObject.name+" found no trailrenderer on paper prefab");				
				
				//Correct color inserted
				if(colorMatch) 
				{
					iTween.MoveTo(paper, iTween.Hash("position", _target.transform.position, "time", _slideTime, "easetype", _easeTypeSlide, 
													"oncomplete", "TriggerDestroyPaper", "oncompleteparams", paper, "oncompletetarget", gameObject));
					
					if(OnCorrectPaperInserted != null)
						OnCorrectPaperInserted();
				}
				//Wrong color inserted
				else
				{
					iTween.MoveTo(paper, iTween.Hash("position", _target.transform.position, "time", _slideTime, "easetype", _easeTypeSlide, 
													"oncomplete", "TriggerIncineratePaper", "oncompleteparams", paper, "oncompletetarget", gameObject));
				}				
			}
			else
			{
				_IsSlideLocked = true;
				
				var paper = (GameObject) Instantiate(_paperlightset[i].paperToSlide, _paperlightset[i].paper.transform.position, _paperlightset[i].paper.transform.rotation);
				paper.transform.parent = _dynamicObjects.transform;
				
				iTween.MoveTo(paper, iTween.Hash("position", _gate.gameObject.transform.position, "time", _slideTime, "easetype", _easeTypeSlide, 
													"oncomplete", "TriggerIncineratePaper", "oncompleteparams", paper, "oncompletetarget", gameObject));			
			}

            InstantiateParticlesAtPaper(_particles.swipe, gameObject);
		}
    }
	
	private void TriggerDestroyPaper(GameObject go)
	{
		StartCoroutine(DestroyPaper(go));
	}
	
	IEnumerator DestroyPaper(GameObject go)
	{
		//Wait for paper to finish sliding
		yield return new WaitForSeconds(_slideWait);
		
		//Instantiate particles
		InstantiateParticles(_particles.complete, go);
		
		//Destroy paper
		Destroy(go);
		//Allow player to slide again
		_IsSlideLocked = false;
	}
	
	private void TriggerIncineratePaper(GameObject go)
	{
		StartCoroutine(IncineratePaper(go));
	}
	
	IEnumerator IncineratePaper(GameObject go)
	{
		//wait or paper to finish sliding
		yield return new WaitForSeconds(_slideWait);
		
		//Instantiate particles
		InstantiateParticles(_particles.failed, go);
		
		//Destroy paper
		Destroy(go);
		//Allow player to slide again
		_IsSlideLocked = false;
	}
	
	//-----------
	public void Reset()
	{
		DisablePaper();
		TurnOfAllLights();
	}
	
	//Method for playing the correct swipe sound
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

    private void InstantiateParticlesAtPaper(GameObject particles, GameObject posRotGO)
    {
        if(particles != null)
        {
            foreach(Transform child in posRotGO.transform)
            {
                if(child.name.Equals("ParticlePaperPos") && particles != null)
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

    public void RollConvoreBelt()
    {
        Vector2 thisOffset = _conveyorBelt.renderer.material.mainTextureOffset;
        _conveyorBelt.renderer.material.mainTextureOffset = (thisOffset + new Vector2(0.0f, -0.01f));
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
        [HideInInspector] public bool isOn = false;
		public GameObject paperToSlide;
    };
	
    [System.Serializable]
    public class Particles
    {
		public GameObject complete;
		public GameObject failed;
        public GameObject swipe;
		public GameObject enablePaper;
		public GameObject disablePaper;
    };
	
    #endregion
}
