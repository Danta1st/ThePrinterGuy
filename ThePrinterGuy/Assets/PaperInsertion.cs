using UnityEngine;
using System.Collections;

public class PaperInsertion : MonoBehaviour
{
    #region Editor Publics
    [SerializeField] private GameObject _gate;
    [SerializeField] private iTween.EaseType _easeTypeOpen  = iTween.EaseType.easeOutCirc;
    [SerializeField] private iTween.EaseType _easeTypeClose = iTween.EaseType.easeOutBounce;
    [SerializeField] private PaperLightSet[] _paperlightset;
    #endregion

    #region Privates
    private bool _isGateOpen    = true;
    private bool _isGateAllowedToRun = false;
    private float _openTime     = 0.5f;
    private float _closeTime    = 0.5f;
    private float _waitTime     = 0.2f;
    #endregion

    #region Delegates & Events
    public delegate void OnPaperInsertedAction();
    public static event OnPaperInsertedAction OnCorrectPaperInserted;
    public static event OnPaperInsertedAction OnIncorrectPaperInserted;
    #endregion

    //TODO: Insert Proper connectivity to the Action Sequencer
    void OnEnable()
    {
        StartGate();
        GestureManager.OnSwipeUp += DisablePaper;
        GestureManager.OnSwipeDown += EnablePaper;
    }
    void OnDisable()
    {
        StopGate();
        GestureManager.OnSwipeUp -= DisablePaper;
        GestureManager.OnSwipeDown -= EnablePaper;

    }

    #region Monobehaviour Functions
	void Start ()
    {
	    InitializeLights();
	}

	void Update ()
    {

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


    //TODO: Paper Methods
    private void DisablePaper()
    {
        for(int i = 0; i < _paperlightset.Length; i++)
        {
            _paperlightset[i].paper.SetActive(false);
        }
    }

    private void EnablePaper()
    {
        for(int i = 0; i < _paperlightset.Length; i++)
        {
            _paperlightset[i].paper.SetActive(true);
        }

    }

    private void InitializePaper()
    {

    }

    private void SlidePaper(int i)
    {

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
