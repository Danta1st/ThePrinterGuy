using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ink : MonoBehaviour 
{
	#region Editor Publics
	[SerializeField] private List<InkCartridgeClass> _machineInks;
	//Smoothing
    [SerializeField] private iTween.EaseType _easeTypeOpen  = iTween.EaseType.easeOutBack;
    [SerializeField] private iTween.EaseType _easeTypeClose = iTween.EaseType.easeInExpo;
	//Particles
	[SerializeField] private Particles _particles;

    #endregion
	
	#region Privates
	//Gate Variables
    private float _openTime     = 0.250f;
    private float _closeTime    = 0.250f;
	
	//Slide variables
	private iTween.EaseType _easeTypeSlide = iTween.EaseType.easeOutExpo;
	private float _inkMoveSpeed		= 0.5f;
	private bool _canSlide = true;
	private bool isOnInk = false;
	
	//References
	private GameObject _particleSmoke;	
	private GameObject _dynamicObjects;
    #endregion
	
	#region Delegates & Events
	public delegate void OnInkInsertedAction();
    public static event OnInkInsertedAction OnCorrectInkInserted;
	#endregion

	void Awake () 
	{
		//Paths for ink sliding
		//Get dynamic object reference
		_dynamicObjects = GameObject.Find("Dynamic Objects");
		
		//Check if _particles if empty, and throw warning if true
		if(_particles.complete == null)
			Debug.LogWarning(gameObject.name+" reported that a particle prefab in '_particles' is empty");
		if(_particles.failed == null)
			Debug.LogWarning(gameObject.name+" reported that a particle prefab in '_particles' is empty");
		if(_particles.enable == null)
			Debug.LogWarning(gameObject.name+" reported that a particle prefab in '_particles' is empty");
		if(_particles.disable == null)
			Debug.LogWarning(gameObject.name+" reported that a particle prefab in '_particles' is empty");
		if(_particles.smoke == null)
			Debug.LogWarning(gameObject.name+" reported that a particle prefab in '_particles' is empty");
		
		//Instantiate smoke
		if(_particles.smoke != null)
		{
			_particleSmoke = (GameObject) Instantiate(_particles.smoke);
			_particleSmoke.transform.parent = _dynamicObjects.transform;
		}
		
		
		foreach(InkCartridgeClass icc in _machineInks)
		{
			//Remember beginPositions - Why?
			icc.insertableStartPos = icc.insertableCartridge.position;
			icc.insertableCartridgeClone = (GameObject)Instantiate(icc.insertableCartridge.gameObject, icc.insertableStartPos,
				icc.insertableCartridge.transform.rotation);
			icc.insertableCartridgeClone.collider.enabled = false;
			icc.insertableCartridgeClone.GetComponent<ItemIdleState>().enabled = false;
			
			//Succes path
			icc.pathSucc = new Vector3[3];
			icc.pathSucc[0] = icc.insertableCartridge.position;
			icc.pathSucc[2] = icc.cartridge.position;
			icc.pathSucc[1] = new Vector3(icc.pathSucc[2].x, icc.pathSucc[2].y, icc.pathSucc[2].z + 0.5f * (icc.pathSucc[0].z - icc.pathSucc[2].z));
			//Fail path
			icc.pathFail = new Vector3[3];
			icc.pathFail[0] = icc.pathSucc[0];
			icc.pathFail[1] = icc.pathSucc[1];
			icc.pathFail[2] = icc.pathSucc[2];
			icc.pathFail[2].z -= 1.5f;
		}	
	}	
	
	void OnEnable()
	{
		StartGates();
		
		BpmSequencer.OnInkNode += StartInkTask;
		BpmSequencerItem.OnFailed += InkReset;
	}
	
	void OnDisable()
	{
		StopGates();
		
		GestureManager.OnSwipeRight -= InsertCartridge;
		BpmSequencer.OnInkNode -= StartInkTask;
		BpmSequencerItem.OnFailed -= InkReset;
		UnsubscribeInkPunch();
	}
	
	void OnDestroy()
	{
		StopGates();
		
		GestureManager.OnSwipeRight -= InsertCartridge;
		BpmSequencer.OnInkNode -= StartInkTask;
		BpmSequencerItem.OnFailed -= InkReset;
		UnsubscribeInkPunch();
	}
	
	#region Class Methods	
	#region Gates and Machines Ink
	// Cartridge gate functions
	private void StartGates()
    {
		BeatController.OnBeat8th7 += CloseGates;
		//TODO: Insert close gate sound?
		BeatController.OnBeat8th3 += OpenGates;
		BeatController.OnBeat8th3 += SoundManager.Effect_Ink_SlotOpen4;
    }

    private void StopGates()
    {
		BeatController.OnBeat8th7 -= CloseGates;
		BeatController.OnBeat8th3 -= OpenGates;
		BeatController.OnBeat8th3 -= SoundManager.Effect_Ink_SlotOpen4;
    }
	
	private void OpenGates()
    {
		foreach(InkCartridgeClass icc in _machineInks)
		{
			GameObject go = icc.lid;
	        if(!icc.lidIsOpen)
	        {				
				if(icc.lidDirection == OpenDirection.Left)
				{
		            iTween.RotateTo(go,iTween.Hash("y", go.transform.localRotation.eulerAngles.y + 90, "time", _openTime,
		                                            "islocal", true, "easetype", _easeTypeOpen));
				}
				else if(icc.lidDirection == OpenDirection.Right)
				{
		            iTween.RotateTo(go,iTween.Hash("y", go.transform.localRotation.eulerAngles.y - 90, "time", _openTime,
		                                            "islocal", true, "easetype", _easeTypeOpen));
				}
				else if(icc.lidDirection == OpenDirection.Up)
				{
		            iTween.RotateTo(go,iTween.Hash("x", go.transform.localRotation.eulerAngles.x - 90, "time", _openTime,
		                                            "islocal", true, "easetype", _easeTypeOpen));
				}
				if(icc.lidDirection == OpenDirection.Down)
				{
		            iTween.RotateTo(go,iTween.Hash("x", go.transform.localRotation.eulerAngles.x + 90, "time", _openTime,
		                                            "islocal", true, "easetype", _easeTypeOpen));
				}
	            icc.lidIsOpen = true;
	        }
		}
    }

    private void CloseGates()
    {
		foreach(InkCartridgeClass icc in _machineInks)
		{
			GameObject go = icc.lid;
	        if(icc.lidIsOpen)
	        {
	            if(icc.lidDirection == OpenDirection.Left)
				{
		            iTween.RotateTo(go,iTween.Hash("y", go.transform.localRotation.eulerAngles.y - 90, "time", _closeTime,
		                                            "islocal", true, "easetype", _easeTypeClose));
				}
				else if(icc.lidDirection == OpenDirection.Right)
				{
		            iTween.RotateTo(go,iTween.Hash("y", go.transform.localRotation.eulerAngles.y + 90, "time", _closeTime,
		                                            "islocal", true, "easetype", _easeTypeClose));
				}
				else if(icc.lidDirection == OpenDirection.Up)
				{
		            iTween.RotateTo(go,iTween.Hash("x", go.transform.localRotation.eulerAngles.x + 90, "time", _closeTime,
		                                            "islocal", true, "easetype", _easeTypeClose));
				}
				if(icc.lidDirection == OpenDirection.Down)
				{
		            iTween.RotateTo(go,iTween.Hash("x", go.transform.localRotation.eulerAngles.x - 90, "time", _closeTime,
		                                            "islocal", true, "easetype", _easeTypeClose));
				}
	            icc.lidIsOpen = false;
	        }
		}
    }
	
	private int _tempPunch = 0;
	private float _punchTime = 0.45f;
	private void SubscribeInkPunch(int itemNumber)
	{
		_tempPunch = itemNumber;
		
		BeatController.OnAll4Beats += PunchInk;
		
	}
	private void UnsubscribeInkPunch()
	{	
		BeatController.OnAll4Beats -= PunchInk;
	}
		
	private void PunchInk()
	{
		if(_tempPunch != null)
			iTween.PunchScale(_machineInks[_tempPunch].insertableCartridge.gameObject, new Vector3(0.2f, 0.2f, 0.2f), _punchTime);
	}
	
	#endregion
	
	#region Insertable Ink
	//POLISH: Remake to instantiate new ink instead of moving current
	private void InsertCartridge(GameObject go)
	{
		if(go == null || !_canSlide)
			return;
				
		//TODO: Needs comments. What is happening? try to clean this method to work on the InkCartridgeClass instead of list?
		InkCartridgeClass currIcc = null;
		InkCartridgeClass icc;
		int j = 0;
		int count = _machineInks.Count;
        int index = 0;

		for(int i = 0; i < count; i++)
		{
			icc = _machineInks[i];
						
			if(icc.insertableCartridge.gameObject == go)
			{
                index = i;
				currIcc = icc;
				break;
			}
			j++;
		}

		
		if(currIcc == null)
			return;

        PlaySwipeSound(index);

		//Succesfull swipe
		if(currIcc.lidIsOpen == true && currIcc.cartridgeEmpty)
		{
			_canSlide = false;	
			isOnInk = false;
			
			//Unsubsribe gesture
			GestureManager.OnSwipeRight -= InsertCartridge;
			
			currIcc.cartridgeEmpty = false;
			currIcc.cartridge.renderer.material.mainTexture = currIcc.full;
			
			//Broadcast task done
	        if(OnCorrectInkInserted != null)
	        {
	            OnCorrectInkInserted();
	        }
						
//			currIcc.insertableCartridge.gameObject.SetActive(false);

			currIcc.insertableCartridgeClone.SetActive(true);

			//Move the ink
			iTween.MoveTo(currIcc.insertableCartridgeClone, iTween.Hash("path", currIcc.pathSucc, 
						  	"easetype", _easeTypeSlide, "time", _inkMoveSpeed, 
							"oncomplete", "InkSuccess", "oncompletetarget", this.gameObject, "oncompleteparams", currIcc));
		}
		//Failed swipe
		else
		{	
			
//			currIcc.insertableCartridge.gameObject.SetActive(false);
			
			currIcc.insertableCartridgeClone.SetActive(true);
			
			iTween.MoveTo(currIcc.insertableCartridgeClone, iTween.Hash("path", currIcc.pathFail, 
						  	"easetype", _easeTypeSlide, "time", _inkMoveSpeed, 
							"oncomplete", "InkFailed", "oncompletetarget", this.gameObject, "oncompleteparams", currIcc));	
		}
	}
	
	private void InkSuccess(InkCartridgeClass icc)
	{
		
		//Instantiate particles
		InstantiateParticles(_particles.complete, icc.cartridge.gameObject);
        InstantiateParticlesToWordPos(_particles.completeClick, icc.cartridge.gameObject);

		//Stop Smoke
		if(_particleSmoke != null)
			_particleSmoke.particleSystem.Stop();		
		
		if(!isOnInk)
        	InkReset();
		
//		icc.insertableCartridge.gameObject.SetActive(true);
		icc.insertableCartridgeClone.transform.position = icc.insertableStartPos;
		icc.insertableCartridgeClone.SetActive(false);
//        icc.insertableCartridge.transform.position = icc.insertableStartPos;
//		icc.insertableCartridge.GetComponent<ItemIdleState>().StartFloat();
		
        _canSlide = true;
	}	
	
	private void InkFailed(InkCartridgeClass icc)
	{
		//Instantiate fail particles
		InstantiateParticlesToWordPos(_particles.failed, icc.cartridge.gameObject);
		//Play sound
        SoundManager.Effect_InGame_Task_Unmatched();
		
//		icc.insertableCartridge.transform.position = icc.insertableStartPos;
//		icc.insertableCartridge.GetComponent<ItemIdleState>().StartFloat();
//		icc.insertableCartridge.gameObject.SetActive(true);
		icc.insertableCartridgeClone.transform.position = icc.insertableStartPos;
		icc.insertableCartridgeClone.SetActive(false);
		
		_canSlide = true;
	}

    private void PlaySwipeSound(int index)
    {
        switch(index)
        {
            case 0:
                SoundManager.Effect_Ink_RightSlot1();
                break;
            case 1:
                SoundManager.Effect_Ink_RightSlot2();
                break;
            case 2:
                SoundManager.Effect_Ink_RightSlot3();
                break;
            case 3:
                SoundManager.Effect_Ink_RightSlot4();
                break;
            default:
                break;
        }
    }
	
	private void InkReset()
	{
		//Unsubscribe from gesture
		GestureManager.OnSwipeRight -= InsertCartridge;
		
		//TODO: Comment this properly
		InkCartridgeClass icc;
		//FIXME: Particles
		if(_particleSmoke != null)
			_particleSmoke.particleSystem.Stop();
		
//		int j = 0;
		for(int i = 0; i < _machineInks.Count; i++)
		{
			icc = _machineInks[i];
			icc.cartridge.gameObject.SetActive(true);
			
			//Instantiate particles
			InstantiateParticles(_particles.disable, icc.insertableCartridge.gameObject);
			
			icc.cartridgeEmpty = false;
			icc.insertableCartridge.gameObject.SetActive(false);
			icc.cartridge.renderer.material.mainTexture = icc.full;

			icc.insertableCartridgeClone.transform.position = icc.insertableStartPos;
			icc.insertableCartridgeClone.SetActive(false);
//			j++;
		}
	}
	
	//public function to retrigger ink task. Used in GUIGameCamera
	public void ReTriggerInkTask()
	{
		StartInkTask(_currentInk);
	}
	
	private int _currentInk = 0;
	private void StartInkTask(int itemNumber)
	{		
		_currentInk = itemNumber;
		isOnInk = true;
		//Subscribe to gesture
		GestureManager.OnSwipeRight += InsertCartridge;
		
		//Activate all cartridges
		foreach(InkCartridgeClass icc in _machineInks)
		{
			//Instantiate particles
			InstantiateParticles(_particles.enable, icc.insertableCartridge.gameObject);
			//Activate cartridge
			icc.insertableCartridge.gameObject.SetActive(true);
		}
		
		//TODO: Comment on why this is necessary if put in again!
//		if(_machineInks.Count < itemNumber + 1)
//		{
//			if(OnCorrectInkInserted != null)
//            {
//                OnCorrectInkInserted();
//            }
//
//			Debug.Log("ERROR INK: Number out of index!");
//			return;
//		}
		
		if(_machineInks[itemNumber].cartridgeEmpty == false)
        {
            EmptyCartridge(itemNumber);
        }
		
		//Randomisation method for ink calls
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
		SubscribeInkPunch(itemNumber);		
	}
	
	private void EmptyCartridge(int iccnumber)
	{
		//FIXME: particles
		foreach(Transform child in _machineInks[iccnumber].cartridge.transform)
		{
			if(child.name.Equals("ParticlePos") && _particleSmoke != null)
			{
				_particleSmoke.transform.position = child.position;
				_particleSmoke.transform.rotation = child.rotation;
				_particleSmoke.particleSystem.Play();
			}
		}
		_machineInks[iccnumber].cartridgeEmpty = true;
		_machineInks[iccnumber].cartridge.renderer.material.mainTexture = _machineInks[iccnumber].empty;
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

    private void InstantiateParticlesToWordPos(GameObject particles, GameObject posRotGO)
 {
     if(particles != null)
     {
         foreach(Transform child in posRotGO.transform)
         {
             if(child.name.Equals("ParticleWordPos") && particles != null)
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
	#endregion
	#endregion
	
	#region SubClasses
	//Cartridge class
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
		
		[HideInInspector] public Vector3[] pathSucc = new Vector3[3];
		[HideInInspector] public Vector3[] pathFail = new Vector3[3];
		[HideInInspector] public Vector3 insertableStartPos;
		[HideInInspector] public bool lidIsOpen = false;
		[HideInInspector] public bool cartridgeEmpty = false;
		[HideInInspector] public GameObject insertableCartridgeClone;
    };
	
	//Particles class
    [System.Serializable]
    public class Particles
    {
		public GameObject enable;
		public GameObject disable;
		public GameObject complete;
        public GameObject completeClick;
		public GameObject failed;
		public GameObject smoke;
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
