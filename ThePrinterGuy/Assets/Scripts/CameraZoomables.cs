using UnityEngine;
using System.Collections;

public class CameraZoomables : MonoBehaviour
{
    
    public float zoomTime = 2.0f;

    private GameObject[] _zoomObjects;
    private Camera _zoomCamera;
    private bool _isZoomed = false;

    void Awake()
    {
        _zoomObjects = GameObject.FindGameObjectsWithTag("Player");
    }

    // Use this for initialization
    void Start()
    {
        _zoomCamera = Camera.main;
    }

    #region OnEnable / Disable
    void OnEnable()
    {
        GestureManager.OnTap += OnTapAction;
        GestureManager.OnDoubleTap += OnDoubleTapAction;
    }

    void OnDisable()
    {
        GestureManager.OnTap -= OnTapAction;
        GestureManager.OnDoubleTap -= OnDoubleTapAction;
    }
    #endregion

    #region TouchEvents
    void OnTapAction(GameObject thisGameObj, Vector2 screenPos)
    {
        if(!_isZoomed)
        {
            foreach(GameObject zoomObj in _zoomObjects)
            {
                if(zoomObj == thisGameObj)
                {
                    zoomObj.GetComponent<CameraZooming>().ZoomIn(_zoomCamera, zoomTime);
                    _isZoomed = true;

                }
            }
        }
    }

    void OnDoubleTapAction(GameObject thisGameObj, Vector2 screenPos)
    {
        if(_isZoomed)
        {
            foreach(GameObject zoomObj in _zoomObjects)
            {
                if(zoomObj == thisGameObj)
                {
                    zoomObj.GetComponent<CameraZooming>().ZoomOut(_zoomCamera, zoomTime);
                    _isZoomed = false;
                }
            }
        }
    }
    #endregion
}
