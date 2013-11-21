using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ink : MonoBehaviour 
{
	#region Editor Publics
	[SerializeField] private List<InkCartridgeClass> _machineInks;
	[SerializeField] private ParticleSystem _particleSystemStars;
	[SerializeField] private ParticleSystem _particleSystemSmoke;
	[SerializeField] private ParticleSystem _particleSystemExplosion;
    [SerializeField] private iTween.EaseType _easeTypeOpen  = iTween.EaseType.easeOutCirc;
    [SerializeField] private iTween.EaseType _easeTypeClose = iTween.EaseType.easeOutBounce;
    #endregion
	
	#region Privates
	//Gate Variables
    private bool _isGateAllowedToRun = false;
    private float _openTime     = 0.250f;
    private float _closeTime    = 0.250f;
    private float _waitTime     = 0.500f;
    private GenericSoundScript GSS;
	
	//Slide variables
	private iTween.EaseType _easeTypeSlide = iTween.EaseType.easeOutExpo;
	private float _inkMoveSpeed		= 0.4f;
	private bool _canSlide = true;
	
	// Particlesystem
	private ParticleSystem _particleStars;
	private ParticleSystem _particleSmoke;
	private ParticleSystem _particleExplosion;
	
	private GameObject _dynamicObjects;
	
	public delegate void OnInkInsertedAction();
    public static event OnInkInsertedAction OnCorrectInkInserted;
    #endregion

	void Awake () 
	{
		_dynamicObjects = GameObject.Find("Dynamic Objects");
		if(_particleSystemSmoke != null)
		{
			_particleSmoke = (ParticleSystem)Instantiate(_particleSystemSmoke);
		}
		else
			Debug.Log("Smoke Particle not loaded for Ink");
		if(_particleSystemStars != null)
		{
			_particleStars = (ParticleSystem)Instantiate(_particleSystemStars);
			_particleStars.renderer.material.shader = Shader.Find("Transparent/Diffuse");
		}
		if(_particleSystemExplosion != null)
		{
			_particleExplosion = (ParticleSystem)Instantiate (_particleSystemExplosion);
		}
		else
			Debug.Log("Star Particle not loaded for Ink");
		
		_particleSmoke.transform.parent = _dynamicObjects.transform;
		_particleStars.transform.parent = _dynamicObjects.transform;

        GSS = transform.GetComponentInChildren<GenericSoundScript>();

		foreach(InkCartridgeClass icc in _machineInks)
		{
			icc.insertableStartPos = icc.insertableCartridge.position;
		}
		
		foreach(InkCartridgeClass icc in _machineInks)
		{
			
			foreach(Transform t in icc.lid.transform) {
				if(t.name == "InkCrashObj") {
					GameObject crashPos;
					crashPos = t.gameObject;
					icc.inkCollisionPosition = crashPos.transform.position;
				}
			}
		}
	}
	
	void OnEnable()
	{
		
		ActionSequencerManager.OnInkNode += StartInkTask;
		ActionSequencerItem.OnFailed += InkReset;
	}
	
	void OnDisable()
	{
		ActionSequencerManager.OnInkNode -= StartInkTask;
		ActionSequencerItem.OnFailed -= InkReset;
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
	
	private void OpenGate(InkCartridgeClass icc)
    {
        GSS.PlayClip(Random.Range(0,3));
		GameObject go = icc.lid;
        if(!icc.lidIsOpen)
        {
			//yield return new WaitForSeconds(_waitTime);
			
			if(icc.lidDirection == OpenDirection.Left)
			{
	            iTween.RotateTo(go,iTween.Hash("y", go.transform.localRotation.eulerAngles.y + 90, "time", _openTime,
	                                            "islocal", true, "easetype", _easeTypeOpen, "oncomplete", "NextAnimation",
	                                            "oncompletetarget", gameObject, "oncompleteparams", icc));
			}
			else if(icc.lidDirection == OpenDirection.Right)
			{
	            iTween.RotateTo(go,iTween.Hash("y", go.transform.localRotation.eulerAngles.y - 90, "time", _openTime,
	                                            "islocal", true, "easetype", _easeTypeOpen, "oncomplete", "NextAnimation",
	                                            "oncompletetarget", gameObject, "oncompleteparams", icc));
			}
			else if(icc.lidDirection == OpenDirection.Up)
			{
	            iTween.RotateTo(go,iTween.Hash("x", go.transform.localRotation.eulerAngles.x - 90, "time", _openTime,
	                                            "islocal", true, "easetype", _easeTypeOpen, "oncomplete", "NextAnimation",
	                                            "oncompletetarget", gameObject, "oncompleteparams", icc));
			}
			if(icc.lidDirection == OpenDirection.Down)
			{
	            iTween.RotateTo(go,iTween.Hash("x", go.transform.localRotation.eulerAngles.x + 90, "time", _openTime,
	                                            "islocal", true, "easetype", _easeTypeOpen, "oncomplete", "NextAnimation",
	                                            "oncompletetarget", gameObject, "oncompleteparams", icc));
			}
            icc.lidIsOpen = true;
        }
    }

    IEnumerator CloseGate(InkCartridgeClass icc)
    {
		GameObject go = icc.lid;
        if(icc.lidIsOpen)
        {
			yield return new WaitForSeconds(_waitTime);
            if(icc.lidDirection == OpenDirection.Left)
			{
	            iTween.RotateTo(go,iTween.Hash("y", go.transform.localRotation.eulerAngles.y - 90, "time", _openTime,
	                                            "islocal", true, "easetype", _easeTypeOpen, "oncomplete", "NextAnimation",
	                                            "oncompletetarget", gameObject, "oncompleteparams", icc));
			}
			else if(icc.lidDirection == OpenDirection.Right)
			{
	            iTween.RotateTo(go,iTween.Hash("y", go.transform.localRotation.eulerAngles.y + 90, "time", _openTime,
	                                            "islocal", true, "easetype", _easeTypeOpen, "oncomplete", "NextAnimation",
	                                            "oncompletetarget", gameObject, "oncompleteparams", icc));
			}
			else if(icc.lidDirection == OpenDirection.Up)
			{
	            iTween.RotateTo(go,iTween.Hash("x", go.transform.localRotation.eulerAngles.x + 90, "time", _openTime,
	                                            "islocal", true, "easetype", _easeTypeOpen, "oncomplete", "NextAnimation",
	                                            "oncompletetarget", gameObject, "oncompleteparams", icc));
			}
			if(icc.lidDirection == OpenDirection.Down)
			{
	            iTween.RotateTo(go,iTween.Hash("x", go.transform.localRotation.eulerAngles.x - 90, "time", _openTime,
	                                            "islocal", true, "easetype", _easeTypeOpen, "oncomplete", "NextAnimation",
	                                            "oncompletetarget", gameObject, "oncompleteparams", icc));
			}
            icc.lidIsOpen = false;
        }
		yield break;
    }
	
	private void NextAnimation(InkCartridgeClass icc)
    {
        if(_isGateAllowedToRun)
        {
			foreach(InkCartridgeClass IC in _machineInks)
			{
				if(IC.lidIsOpen && icc == IC)
	                StartCoroutine_Auto(CloseGate(IC));
	            else if(!IC.lidIsOpen && icc == IC)
	                OpenGate(IC);
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
            OpenGate(icc);
	}
	#endregion
	
	#region Insertable Ink
	private void InsertCartridge(GameObject go)
	{
		if(go == null || !_canSlide)
			return;
		InkCartridgeClass currIcc = null;
		InkCartridgeClass icc;
		int j = 0;
		int count = _machineInks.Count;
		for(int i = 0; i < count; i++)
		{
			icc = _machineInks[j];
			if(icc.cartridge == null)
			{
				_machineInks.Remove(icc);
				_machineInks.TrimExcess();
				continue;	
			}
			if(icc.insertableCartridge.gameObject == go)
			{
				currIcc = icc;
				break;
			}
			j++;
		}
		
		if(currIcc == null)
			return;
		
		if(currIcc.lidIsOpen == true && currIcc.cartridgeEmpty)
		{
			_canSlide = false;
			iTween.MoveTo(currIcc.insertableCartridge.gameObject, iTween.Hash("position", currIcc.cartridge.transform.position, 
						  "easetype", _easeTypeSlide, "time", _inkMoveSpeed, "oncomplete", "InkSuccess", "oncompletetarget", this.gameObject, "oncompleteparams", currIcc));
			
			if(OnCorrectInkInserted != null)
            {
                OnCorrectInkInserted();
            }
		}
		else
		{
			iTween.MoveTo(currIcc.insertableCartridge.gameObject, iTween.Hash("position", currIcc.inkCollisionPosition, 
							  "easetype", _easeTypeSlide, "time", _inkMoveSpeed, "oncomplete", "InkFailed", "oncompletetarget", this.gameObject, "oncompleteparams", currIcc));			
//				foreach(Transform t in currIcc.lid.transform)
//			{
//				if(t.name == "InkCrashObj")
//				{
//					GameObject target = t.gameObject;
//					iTween.MoveTo(currIcc.insertableCartridge.gameObject, iTween.Hash("position", target.transform.position, 
//							  "easetype", _easeTypeSlide, "time", _inkMoveSpeed, "oncomplete", "InkFailed", "oncompletetarget", this.gameObject, "oncompleteparams", currIcc));
//				}
//			}
				
			
		}
	}
	
	private void InkSuccess(InkCartridgeClass icc)
	{
		icc.cartridgeEmpty = false;
		icc.cartridge.gameObject.SetActive(true);
		if(_particleSmoke != null && _particleSmoke.isPlaying)
			_particleSmoke.Stop();
		foreach(Transform child in icc.cartridge.transform)
		{
			if(child.name.Equals("ParticlePos") && _particleStars != null)
			{
				_particleStars.transform.position = child.position;
				_particleStars.transform.rotation = child.rotation;
				_particleStars.Play();
			}
		}
		GestureManager.OnSwipeRight -= InsertCartridge;
		icc.insertableCartridge.position = icc.insertableStartPos;
		_canSlide = true;
	}
	
	private IEnumerator InkFailed(InkCartridgeClass icc)
	{
		if(_particleExplosion != null && _particleExplosion.isPlaying)
			_particleExplosion.Stop();
		
		_particleExplosion.Play();
		_particleExplosion.transform.position = icc.insertableCartridge.position;
		icc.insertableCartridge.transform.position = icc.insertableStartPos;
		yield return new WaitForSeconds(_particleExplosion.duration);
		_canSlide = true;
	}
	
	private void InkReset()
	{
		InkCartridgeClass icc;
		if(_particleSmoke != null && _particleSmoke.isPlaying)
			_particleSmoke.Stop();
		int j = 0;
		for(int i = 0; i < _machineInks.Count; i++)
		{
			icc = _machineInks[j];
			if(icc.cartridge == null)
			{
				_machineInks.Remove(icc);
				_machineInks.TrimExcess();
				continue;	
			}
			icc.cartridge.gameObject.SetActive(true);
			icc.cartridgeEmpty = false;
			icc.insertableCartridge.gameObject.SetActive(false);
			j++;
		}
		GestureManager.OnSwipeRight -= InsertCartridge;
	}
	
	private void StartInkTask(int itemNumber)
	{
		foreach(InkCartridgeClass icc in _machineInks)
		{
			icc.insertableCartridge.gameObject.SetActive(true);
		}
		if(_machineInks.Count < itemNumber + 1)
		{
			if(OnCorrectInkInserted != null)
            {
                OnCorrectInkInserted();
            }

			Debug.Log("ERROR INK: Number out of index!");
			return;
		}
		
		if(_machineInks[itemNumber].cartridgeEmpty == false)
        {
            EmptyCartridge(itemNumber);
        }
		
		/*var identifier = Random.Range(0,_machineInks.Count);
		
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
        }*/
		
		GestureManager.OnSwipeRight += InsertCartridge;
		StartGates();
	}
	
	private void EmptyCartridge(int iccnumber)
	{
		foreach(Transform child in _machineInks[iccnumber].cartridge.transform)
		{
			if(child.name.Equals("ParticlePos") && _particleSmoke != null)
			{
				_particleSmoke.transform.position = child.position;
				_particleSmoke.transform.rotation = child.rotation;
				_particleSmoke.Play();
			}
		}
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
		public OpenDirection lidDirection;
		
		[HideInInspector]
		public Vector3 insertableStartPos;
		[HideInInspector]
        public bool lidIsOpen = false;
		[HideInInspector]
		public bool cartridgeEmpty = false;
		[HideInInspector]
		public Vector3 inkCollisionPosition;
    };
	
	public enum OpenDirection
	{
		Up,
		Down,
		Left,
		Right
	}
    #endregion
}
