using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZoomHandler : MonoBehaviour
{
    #region Editor Publics
    [SerializeField]
    private float _zoomTime = 2.0f;
    #endregion

    #region Privates
    private Camera _zoomCamera;
    private static bool _canZoom = true;
    private bool _isZoomed = false;
    private GameObject _tmpObj;
	private List<GameObject> _zoomables = new List<GameObject>();
    #endregion
	
	#region Delegates & Events
	public delegate void OnGoingFreeroamAction();
	public static event OnGoingFreeroamAction OnGoingFreeroam;
	
	public delegate void OnFreeroamAction();
	public static event OnFreeroamAction OnFreeroam;
	
	public delegate void OnJamTask();
	public static event OnJamTask OnJam;
	
	public delegate void OnTrayTask();
	public static event OnTrayTask OnTray;
	
	public delegate void OnInkTask();
	public static event OnInkTask OnInk;
	
	public delegate void OnPopoutTask();
	public static event OnPopoutTask onPopout;
	#endregion

    void Awake()
    {
		_zoomables.Add(GameObject.FindGameObjectWithTag("TrayTask"));
		_zoomables.Add(GameObject.FindGameObjectWithTag("InkTask"));
		_zoomables.Add(GameObject.FindGameObjectWithTag("JamTask"));
		_zoomables.Add(GameObject.FindGameObjectWithTag("PopoutTask"));
    }
	 
    void Start()
    {
        _zoomCamera = Camera.main;
		//Camera.main.transform.LookAt(Vector3.zero);
    }

    #region OnEnable / Disable
    void OnEnable()
    {
        GestureManager.OnTap += GoToTask;
//        GestureManager.OnDoubleTap += OnDoubleTapAction;
		GestureManager.OnSwipeDown += GoToFreeRoam;
    }

    void OnDisable()
    {
        GestureManager.OnTap -= GoToTask;
//        GestureManager.OnDoubleTap -= OnDoubleTapAction;
		GestureManager.OnSwipeDown -= GoToFreeRoam;
    }
    #endregion

    #region TouchEvents
	private void CheckSwitch(GameObject go)
	{
		switch(go.tag)
		{
		case "TrayTask":
			SetObjectsIn(go);
			if(OnTray != null)
				OnTray();
			//Debug.Log("Cam - OnTray()");
			break;
		case "InkTask":
			SetObjectsIn(go);
			if(OnInk != null)
				OnInk();
			//Debug.Log("Cam - OnInk()");
			break;
		case "JamTask":
			SetObjectsIn(go);
			if(OnJam != null)
				OnJam();
			//Debug.Log("Cam - OnJam()");
			break;
		case "PopoutTask":
			SetObjectsIn(go);
			if(onPopout != null)
				onPopout();
			//Debug.Log("Cam - OnPopout()");
			break;
		default:
			break;
		}
	}
	
	private void SetObjectsIn(GameObject tmpObj)
	{
        _tmpObj = tmpObj;
        _canZoom = false;
        _tmpObj.GetComponent<ZoomController>().ZoomIn(_zoomCamera, _zoomTime);
        _isZoomed = true;
	}
	
	private void SetObjectsOut()
	{
        _canZoom = false;
        _tmpObj.GetComponent<ZoomController>().ZoomOut(_zoomCamera, _zoomTime);
        _isZoomed = false;
        _tmpObj = null;
	}
	
	//Zoom in on Task
    private void GoToTask(GameObject thisGameObj, Vector2 screenPos)
    {
        if(_canZoom && !_isZoomed)
        {
			foreach(GameObject go in _zoomables)
			{
				if(go != null && thisGameObj != null)
				{
					if(go.tag == thisGameObj.tag){
						CheckSwitch(go);
					}
				}
			}
        }
    }
	
	//Zoom out from task
	private void GoToFreeRoam()
    {
        if(_canZoom && _isZoomed)
        {
			SetObjectsOut();
			
			if(OnGoingFreeroam != null)
				OnGoingFreeroam();
			
			Invoke("CallOnFreeroam",_zoomTime);
        }
    }
	
	private void CallOnFreeroam()
	{
		if(OnFreeroam != null)
			OnFreeroam();
	}
	
    #endregion
	

    public static void SetAnimationReady()
    {
        _canZoom = true;
    }
}