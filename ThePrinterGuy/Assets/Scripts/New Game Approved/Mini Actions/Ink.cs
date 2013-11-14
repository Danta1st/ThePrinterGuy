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
    private float _waitTime     = 1f;
	
	//Slide variables
	private iTween.EaseType _easeTypeSlide = iTween.EaseType.easeOutExpo;
	private float _inkMoveSpeed		= 0.4f;
	
	public delegate void OnInkInsertedAction();
    public static event OnInkInsertedAction OnCorrectInkInserted;
    #endregion

	void Awake () 
	{
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
		ActionSequencerItem.OnFailed += InkReset;
	}
	
	void OnDisable()
	{
		ActionSequencerManager.OnInkNode -= StartInkTask;
		ActionSequencerItem.OnFailed += InkReset;
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
			
            iTween.MoveTo(go,iTween.Hash("x", go.transform.localPosition.x - 3, "time", _openTime,
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
            iTween.MoveTo(go,iTween.Hash("x", go.transform.localPosition.x + 3, "time", _closeTime,
                                            "islocal", true, "easetype", _easeTypeClose, "oncomplete", "NextAnimation",
                                            "oncompletetarget", gameObject));
            icc.lidIsOpen = false;
        }
		yield break;
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
	private void InsertCartridge(GameObject go, Vector2 lol)
	{
		if(go == null)
			return;
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
			
			if(OnCorrectInkInserted != null)
				OnCorrectInkInserted();
		}
		else
		{
			// Hit the wall lid and go back?
		}
	}
	
	private void InkSuccess(InkCartridgeClass icc)
	{
		icc.cartridgeEmpty = false;
		icc.cartridge.gameObject.SetActive(true);
		GestureManager.OnTap -= InsertCartridge;
		//GestureManager.OnSwipeRight -= InsertCartridge;
		icc.insertableCartridge.position = icc.insertableStartPos;
		
	}
	
	private void InkReset()
	{
		foreach(InkCartridgeClass icc in _machineInks)
		{
			icc.cartridgeEmpty = false;
			icc.cartridge.gameObject.SetActive(true);
			icc.insertableCartridge.gameObject.SetActive(false);
		}
		GestureManager.OnTap -= InsertCartridge;
		//GestureManager.OnSwipeRight -= InsertCartridge;
	}
	
	private void StartInkTask()
	{
		foreach(InkCartridgeClass icc in _machineInks)
		{
			icc.insertableCartridge.gameObject.SetActive(true);
		}
		var identifier = Random.Range(0,_machineInks.Count);
		
        for(int i = 0; i < _machineInks.Count; i++)
        {
            if(_machineInks[identifier].cartridgeEmpty == false)
            {
                EmptyCartridge(identifier);
                break;
            }
            identifier++;

            if(identifier == _machineInks.Count)
                identifier = 0;
        }
		
		GestureManager.OnTap += InsertCartridge;
		//GestureManager.OnSwipeRight += InsertCartridge;
		StartGates();
	}
	
	private void EmptyCartridge(int iccnumber)
	{
		_machineInks[iccnumber].cartridgeEmpty = true;
		_machineInks[iccnumber].cartridge.gameObject.SetActive(false);
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
