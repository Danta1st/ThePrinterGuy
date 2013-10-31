using UnityEngine;
using System.Collections;

public class CameraZooming : MonoBehaviour
{
    #region Editor Publics
    [SerializeField]
    private float _zoomFieldOfView = 20.0f;
    [SerializeField]
    private float _fieldOfViewTimeAdjustment = 0.5f;
    //[SerializeField]
    private string _zoomPointName = "CameraZoomPosition";
    #endregion

    #region Privates
    private Transform _movePointTransform;
    private Vector3 _movePoint;
    private Vector3 _originalPos;
    private float _originalFOV;
    private bool _isReady = true;
    private bool _isZoomed = false;
    private bool _canFOV;
    private float _fovIterator;
    private Camera _fovCam;
    private float _fovStart;
    private float _fovEnd;
    private float _fovTime;
    private GameObject _lookTarget;
    #endregion

    void Awake()
    {
        _movePointTransform = gameObject.transform.FindChild(_zoomPointName);

        if(_movePointTransform == null)
        {
            Debug.Log("No child transforms found for " + _zoomPointName);
        }
    }

    // Use this for initialization
    void Start()
    {
        _movePoint = _movePointTransform.position;
        _lookTarget = new GameObject();
        //_lookTarget.transform.parent = gameObject.transform;
        //_lookTarget.name = _lookTarget.transform.parent.gameObject.name + " LookTarget";
    }

    void Update()
    {
        if(_canFOV)
        {
            if(_fovIterator < 1.0f)
            {
                _fovIterator += Time.deltaTime * (_fovTime * _fieldOfViewTimeAdjustment);
                _fovCam.fieldOfView = Mathf.Lerp(_fovStart, _fovEnd, _fovIterator);
            }
            else if(_fovIterator >= 1.0f)
            {
                _canFOV = false;
            }
        }
    }

    #region Zoom Functionality
    public void ZoomIn(Camera zoomCamera, float zoomTime)
    {
        if(_isReady && !_isZoomed)
        {
            _isReady = false;

            _originalPos = zoomCamera.transform.position;
            _originalFOV = zoomCamera.fieldOfView;

            iTween.MoveTo(zoomCamera.gameObject, iTween.Hash("position", _movePoint, "looktarget", _lookTarget.transform,
                "time", zoomTime, "easetype", iTween.EaseType.easeInOutCubic, "oncomplete", "ZoomAvailability", "oncompletetarget", gameObject));

            ChangeFieldOfView(zoomCamera, _originalFOV, _zoomFieldOfView, zoomTime);

            iTween.MoveTo(_lookTarget, iTween.Hash("position", gameObject.transform.position, "time", zoomTime, "easetype", iTween.EaseType.easeInOutCubic));
        }
    }

    public void ZoomOut(Camera zoomCamera, float zoomTime)
    {
        if(_isReady && _isZoomed)
        {
            _isReady = false;

            iTween.MoveTo(zoomCamera.gameObject, iTween.Hash("position", _originalPos, "looktarget", _lookTarget.transform,
                "time", zoomTime, "easetype", iTween.EaseType.easeInOutCubic, "oncomplete", "ZoomAvailability", "oncompletetarget", gameObject));

            ChangeFieldOfView(zoomCamera, _zoomFieldOfView, _originalFOV, zoomTime);

            iTween.MoveTo(_lookTarget, iTween.Hash("position", gameObject.transform.parent.position, "time", zoomTime, "easetype", iTween.EaseType.easeInOutCubic));
        }
    }
    #endregion

    #region Functions used for zooming
    private void ZoomAvailability()
    {
        if(!_isZoomed)
        {
            _isZoomed = true;
            _fovCam.transform.LookAt(gameObject.transform.position);
        }
        else if(_isZoomed)
        {
            _isZoomed = false;
            _fovCam.transform.LookAt(gameObject.transform.parent.position);
        }

        _isReady = true;
    }

    private void ChangeFieldOfView(Camera zoomCam, float startFOV, float endFOV, float time)
    {
        _fovIterator = 0.0f;
        _fovCam = zoomCam;
        _fovStart = startFOV;
        _fovEnd = endFOV;
        _fovTime = time;
        _canFOV = true;
    }
    #endregion
}