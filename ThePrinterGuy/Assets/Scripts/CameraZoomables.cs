using UnityEngine;
using System.Collections;

public class CameraZoomables : MonoBehaviour
{
    #region Editor Publics
    [SerializeField]
    private float _zoomTime = 2.0f;
    [SerializeField]
    //private int _layerID = 8;
    #endregion

    #region Privates
    private GameObject[] _zoomObjects;
    private Camera _zoomCamera;
    private bool _isZoomed = false;
    #endregion

    void Awake()
    {
        //_zoomObjects = FindGameObjectsWithLayer(_layerID);
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
    private void OnTapAction(GameObject thisGameObj, Vector2 screenPos)
    {
        if(!_isZoomed)
        {
            foreach (GameObject zoomObject in _zoomObjects)
            {
                if( thisGameObj == zoomObject)
                {
                    thisGameObj.GetComponent<CameraZooming>().ZoomIn(_zoomCamera, _zoomTime);
                    _isZoomed = true;
                }
            }
        }
    }

    private void OnDoubleTapAction(GameObject thisGameObj, Vector2 screenPos)
    {
        if(_isZoomed)
        {
            foreach (GameObject zoomObject in _zoomObjects)
            {
                if( thisGameObj == zoomObject)
                {
                    thisGameObj.GetComponent<CameraZooming>().ZoomOut(_zoomCamera, _zoomTime);
                    _isZoomed = false;
                }
            }
        }
    }
    #endregion

    /*
    GameObject[] FindGameObjectsWithLayer(int layerH)
    {
        GameObject[] allGameObjectsArray = (GameObject[]) Object.FindObjectsOfType(typeof(GameObject));

        System.Collections.Generic.List<GameObject> layerObjectsList
            = new System.Collections.Generic.List<GameObject>();

        for (var i = 0; i < allGameObjectsArray.Length; i++)
        {
            Debug.Log("GOGOGO!!!");
            if(allGameObjectsArray[i].layer == layerH)
            {
                layerObjectsList.Add(allGameObjectsArray[i]);
            }
        }

        if(layerObjectsList.Count == 0)
        {
            Debug.Log("No objects with layer " + _layerID);
            return null;
        }

        return layerObjectsList.ToArray();
    }
    */
}