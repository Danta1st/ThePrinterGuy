using UnityEngine;
using System.Collections;

public class CameraZooming : MonoBehaviour
{
    #region Editor Publics
    [SerializeField]
    private float newFieldOfView = 20.0f;
    #endregion

    #region Privates
    private Transform _movePointTransform;
    private Vector3 _movePoint;
    private Vector3 _originalPos;
    private float _originalFOV;
    private bool _isReady = true;
    private bool _isZoomed = false;
    #endregion

    void Awake()
    {
        _movePointTransform = gameObject.transform.FindChild("Point");

        if(_movePointTransform == null)
        {
            Debug.Log("No child transforms found for Point");
        }
    }

    // Use this for initialization
    void Start()
    {
        _movePoint = _movePointTransform.position;
    }

    void ZoomAvailability()
    {
        if(!_isZoomed)
        {
            _isZoomed = true;
        }
        else if(_isZoomed)
        {
            _isZoomed = false;
        }

        _isReady = true;
    }

    #region Zoom Functionality
    public void ZoomIn(Camera zoomCamera, float zoomTime)
    {
        if(_isReady && !_isZoomed)
        {
            _isReady = false;

            _originalPos = zoomCamera.transform.position;
            _originalFOV = zoomCamera.fieldOfView;

            iTween.MoveTo(zoomCamera.gameObject, iTween.Hash("position", _movePoint, "looktarget", gameObject.transform,
                "time", zoomTime, "easetype", iTween.EaseType.easeInOutCubic, "oncomplete", "ZoomAvailability", "oncompletetarget", gameObject));

            zoomCamera.fieldOfView = Mathf.Lerp(_originalFOV, newFieldOfView, Time.time);
        }
    }

    public void ZoomOut(Camera zoomCamera, float zoomTime)
    {
        if(_isReady && _isZoomed)
        {
            _isReady = false;

            iTween.MoveTo(zoomCamera.gameObject, iTween.Hash("position", _originalPos, "looktarget", gameObject.transform.parent,
                "time", zoomTime, "easetype", iTween.EaseType.easeInOutCubic, "oncomplete", "ZoomAvailability", "oncompletetarget", gameObject));

            zoomCamera.fieldOfView = Mathf.Lerp(newFieldOfView, _originalFOV, Time.time);
        }
    }
    #endregion
}
