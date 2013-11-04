using UnityEngine;
using System.Collections;

public class ZoomController : MonoBehaviour
{
    #region Editor Publics
    [SerializeField]
    private float _zoomFieldOfView = 20.0f;
    #endregion

    #region Privates
	private GameObject _DynamicObjects;
	
    private string _cameraZoomPositionName = "CamPoint";
    private Transform _movePointTransform;
    private Vector3 _movePoint;
	
    private Vector3 _originalPos;
    private float _standardFOV;
	
    private bool _isReady = true;
    private bool _isZoomed = false;
    private GameObject _lookTarget;
    #endregion
	 
	#region Monobehaviour Functions
    void Awake()
    {
		_DynamicObjects = GameObject.Find("Dynamic Objects");
        _movePointTransform = transform.FindChild(_cameraZoomPositionName);
    }

    void Start()
    {
        _standardFOV = Camera.main.fieldOfView;
        _movePoint = _movePointTransform.position;
        _lookTarget = new GameObject();
		_lookTarget.name = "ZoomControllerPoint";
		_lookTarget.transform.parent = _DynamicObjects.transform;
    }
	#endregion

    #region Class Methods
    public void ZoomIn(Camera zoomCamera, float zoomTime)
    {
        if(_isReady && !_isZoomed)
        {
            _isReady = false;

            _originalPos = zoomCamera.transform.position;

            iTween.MoveTo(zoomCamera.gameObject, iTween.Hash("position", _movePoint, "looktarget", _lookTarget.transform,
                												"time", zoomTime, "easetype", iTween.EaseType.easeInOutCubic, 
																"oncomplete", "ZoomAvailability", "oncompletetarget", gameObject));

			AdjustFOV(Camera.main.fieldOfView,_zoomFieldOfView,zoomTime);
			
            iTween.MoveTo(_lookTarget, iTween.Hash("position", transform.position, "time", zoomTime, 
														"easetype", iTween.EaseType.easeInOutCubic));
        }
    }

    public void ZoomOut(Camera zoomCamera, float zoomTime)
    {
        if(_isReady && _isZoomed)
        {
            _isReady = false;

            iTween.MoveTo(zoomCamera.gameObject, iTween.Hash("position", _originalPos, "looktarget", _lookTarget.transform,
                												"time", zoomTime, "easetype", iTween.EaseType.easeInOutCubic, 
																"oncomplete", "ZoomAvailability", "oncompletetarget", gameObject));

			AdjustFOV(Camera.main.fieldOfView,_standardFOV,zoomTime);

            iTween.MoveTo(_lookTarget, iTween.Hash("position", transform.parent.position, "time", zoomTime, 
														"easetype", iTween.EaseType.easeInOutCubic));
        }
    }
	
    private void ZoomAvailability()
    {
        if(!_isZoomed)
        {
            _isZoomed = true;
            ZoomHandler.SetAnimationReady();

        }
        else if(_isZoomed)
        {
            _isZoomed = false;
            ZoomHandler.SetAnimationReady();
        }

        _isReady = true;
    }
	 
	private void AdjustFOV(float start, float end, float time)
	{
		var i = 0.0f;
		var rate = 1.0f/time;
		
		while(i <= 1.0f)
		{
			i += Time.deltaTime * rate;
			Camera.main.fieldOfView = Mathf.Lerp(start,end,i);
			break;
		}
	}
    #endregion
}