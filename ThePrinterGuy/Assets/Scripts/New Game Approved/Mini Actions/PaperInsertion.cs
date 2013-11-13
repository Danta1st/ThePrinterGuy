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
		ActionSequencerManager.OnFirstNode += CheckFirst;
		
        StartGate();
    }
    void OnDisable()
    {
		ActionSequencerManager.OnPaperNode -= TriggerLight;
		ActionSequencerManager.OnPaperNode -= EnablePaper;
		ActionSequencerItem.OnFailed -= Reset;
		ActionSequencerManager.OnFirstNode -= CheckFirst;
		
        StopGate();
    }

    #region Monobehaviour Functions
	void Awake()
	{
		_dynamicObjects = GameObject.Find("Dynamic Objects");	
		InitializeLights();
		DisablePaper();
	}
    #endregion

    #region Class Methods
	private void CheckFirst(string taskname)
	{
		if(taskname == "Paper")
		{
			TriggerLight();
			EnablePaper();
		}
	}
	
	
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
            iTween.MoveTo(_gate,iTween.Hash("y", _gate.transform.localPosition.y + 3, "time", _openTime,
                                            "islocal", true, "easetype", _easeTypeOpen, "oncomplete", "NextAnimation",
                                            "oncompletetarget", gameObject));
            _isGateOpen = true;
        }
    }

    private void CloseGate()
    {
        if(_isGateOpen)
        {
            iTween.MoveTo(_gate,iTween.Hash("y", _gate.transform.localPosition.y - 3, "time", _closeTime,
                                            "islocal", true, "easetype", _easeTypeClose, "oncomplete", "NextAnimation",
                                            "oncompletetarget", gameObject));
            _isGateOpen = false;
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

    private void TriggerLight() //Trigger 1 random light
    {
        var identifier = Random.Range(0,_paperlightset.Length);

        for(int i = 0; i < _paperlightset.Length; i++)
        {
            if(_paperlightset[i].isOn == false)
            {
                TurnOnLight(i);
                break;
            }
            identifier++;

            if(identifier == _paperlightset.Length)
                identifier = 0;
        }
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

    private void EnablePaper()
    {
		GestureManager.OnSwipeUp += TriggerSlide;
        for(int i = 0; i < _paperlightset.Length; i++)
        {
            _paperlightset[i].paper.SetActive(true);
        }
    }
	
	private void TriggerSlide(GameObject go)
	{
		if(go != null)
		{
	        for(int i = 0; i < _paperlightset.Length; i++)
	        {
	            if(_paperlightset[i].isOn && _paperlightset[i].paper.transform == go.transform.parent)
				{
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
				
				iTween.MoveTo(paper, iTween.Hash("position", _target.transform.position, "time", _slideTime, "easetype", _easeTypeSlide, 
													"oncomplete", "DestroyPaper", "oncompleteparams", paper, "oncompletetarget", gameObject));
				
				if(OnCorrectPaperInserted != null)
					OnCorrectPaperInserted();
				
				//DisablePaper();
				//TurnOfAllLights();
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
		GestureManager.OnSwipeUp -= TriggerSlide;
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
