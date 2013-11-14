using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ink : MonoBehaviour 
{
	#region Editor Publics
	[SerializeField] private List<InkCartridgeClass> _machineInks;
    [SerializeField] private iTween.EaseType _easeTypeOpen  = iTween.EaseType.easeOutCirc;
    [SerializeField] private iTween.EaseType _easeTypeClose = iTween.EaseType.easeOutBounce;
    #endregion
	
	#region Privates
	//Gate Variables
    private bool _isGateAllowedToRun = false;
    private float _openTime     = 0.5f;
    private float _closeTime    = 0.5f;
    private float _waitTime     = 2f;
	
	//Slide variables
	private iTween.EaseType _easeTypeSlide = iTween.EaseType.easeOutExpo;
	private bool _IsSlideLocked		= false;
	private float _inkMoveSpeed		= 0.4f;
	private float _slideWait		= 0.2f;
	
	//Whatever
	private GameObject _dynamicObjects;
    #endregion

	void Awake () 
	{
		_dynamicObjects = GameObject.Find("Dynamic Objects");
		foreach(InkCartridgeClass icc in _machineInks)
		{
			icc.insertableStartPos = icc.insertableCartridge.position;
		}
	}
	
	void Update () 
	{
		
	}
	
	void OnEnable()
	{
		ActionSequencerManager.OnInkNode += StartInkTask;
	}
	
	void OnDisable()
	{
		ActionSequencerManager.OnInkNode -= StartInkTask;
	}
	
	#region Private Methods
	#region Gates and Machines Ink
	// Cartridge gate functions
	private void StartGates()
    {
        if(!_isGateAllowedToRun)
        {
            _isGateAllowedToRun = true;
			foreach(InkCartridgeClass icc in _machineInks)
			{
				StartCoroutine_Auto(InitiateInkGates(icc));
			}
        }
		
    }

    private void StopGates()
    {
        _isGateAllowedToRun = false;
    }
	
	IEnumerator OpenGate(InkCartridgeClass icc)
    {
		GameObject go = icc.lid;
        if(!icc.lidIsOpen)
        {
			yield return new WaitForSeconds(_waitTime);
			
            iTween.MoveTo(go,iTween.Hash("x", go.transform.localPosition.x - 10, "time", _openTime,
                                            "islocal", true, "easetype", _easeTypeOpen, "oncomplete", "NextAnimation",
                                            "oncompletetarget", gameObject));
            icc.lidIsOpen = true;
        }
    }

    IEnumerator CloseGate(InkCartridgeClass icc)
    {
		GameObject go = icc.lid;
        if(icc.lidIsOpen)
        {
			yield return new WaitForSeconds(_waitTime);
            iTween.MoveTo(go,iTween.Hash("x", go.transform.localPosition.x + 10, "time", _closeTime,
                                            "islocal", true, "easetype", _easeTypeClose, "oncomplete", "NextAnimation",
                                            "oncompletetarget", gameObject));
            icc.lidIsOpen = false;
        }
    }
	
	private void NextAnimation()
    {
        if(_isGateAllowedToRun)
        {
			foreach(InkCartridgeClass IC in _machineInks)
			{
				if(IC.lidIsOpen)
	                StartCoroutine_Auto(CloseGate(IC));
	            else
	                StartCoroutine_Auto(OpenGate(IC));
			}
        }
    }
	// END OF Gate functions
	
	IEnumerator InitiateInkGates(InkCartridgeClass icc)
	{
		yield return new WaitForSeconds(icc.startWait);
		if(icc.lidIsOpen)
            StartCoroutine_Auto(CloseGate(icc));
        else
            StartCoroutine_Auto(OpenGate(icc));
	}
	#endregion
	
	#region Insertable Ink
	private void ResetCartridges()
	{
		
	}
	
	private void InsertCartridge(GameObject go)
	{
		InkCartridgeClass currIcc = null;
		foreach(InkCartridgeClass icc in _machineInks)
		{
			if(icc.insertableCartridge.gameObject == go)
			{
				currIcc = icc;
				break;
			}
		}
		if(currIcc == null)
			return;
		if(currIcc.lidIsOpen == true && currIcc.cartridgeEmpty)
		{
			iTween.MoveTo(currIcc.insertableCartridge.gameObject, iTween.Hash("position", currIcc.cartridge.transform.position, 
						  "easetype", _easeTypeSlide, "time", _inkMoveSpeed, "oncomplete", "InkSuccess", "oncompletetarget", this.gameObject, "oncompleteparams", currIcc));
		}
		else
		{
			// Hit the wall lid and go back?
		}
	}
	
	private void InkSuccess(InkCartridgeClass icc)
	{
		icc.cartridgeEmpty = false;
		GestureManager.OnSwipeRight -= InsertCartridge;
		icc.insertableCartridge.position = icc.insertableStartPos;
		
	}
	
	private void StartInkTask()
	{
		_isGateAllowedToRun = true;
		GestureManager.OnSwipeRight += InsertCartridge;
		StartGates();
	}
	
	#endregion
	#endregion
	#region SubClasses
    [System.Serializable]
    public class InkCartridgeClass
    {
        public Transform cartridge;
		public Transform insertableCartridge;
        public Texture full;
        public Texture empty;
        public GameObject lid;
		public float startWait = 1f;
		
		[HideInInspector]
		public Vector3 insertableStartPos;
		[HideInInspector]
        public bool lidIsOpen = false;
		[HideInInspector]
		public bool cartridgeEmpty = false;
    };
    #endregion
}
