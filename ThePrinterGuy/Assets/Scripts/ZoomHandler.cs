using UnityEngine;
using System.Collections;

public class ZoomHandler : MonoBehaviour
{
    #region Editor Publics
    [SerializeField]
    private float _zoomTime = 2.0f;
    #endregion

    #region Privates
    private GameObject[] _zoomObjects;
    private Camera _zoomCamera;
    private static bool _canZoom = true;
    private bool _isZoomed = false;
    private GameObject _tmpObj;
    #endregion

    void Awake()
    {
        _zoomObjects = GameObject.FindGameObjectsWithTag("Zoomable");
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
        if(_canZoom && !_isZoomed)
        {

            foreach(GameObject zoomObject in _zoomObjects)
            {
                if(thisGameObj == zoomObject)
                {
                    _tmpObj = thisGameObj;
                    _canZoom = false;
                    _tmpObj.GetComponent<ZoomController>().ZoomIn(_zoomCamera, _zoomTime);
                    _isZoomed = true;
                }
            }

        }
    }

    private void OnDoubleTapAction(GameObject thisGameObj, Vector2 screenPos)
    {
        if(_canZoom && _isZoomed)
        {
            GameObject tmpObj;
            foreach(GameObject zoomObject in _zoomObjects)
            {
                if(thisGameObj == zoomObject && thisGameObj == _tmpObj)
                {
                    _canZoom = false;
                    _tmpObj.GetComponent<ZoomController>().ZoomOut(_zoomCamera, _zoomTime);
                    _isZoomed = false;
                    _tmpObj = null;
                }
            }

        }
    }
    #endregion

    public static void SetAnimationReady()
    {
        _canZoom = true;
    }

    //Not used, should return the GameObjects with a specific layer - for identification of zoomable targets.
    GameObject[] FindGameObjectsWithLayer(int thisLayer)
    {
        GameObject[] allGameObjectsArray = (GameObject[]) Object.FindObjectsOfType(typeof(GameObject));

        System.Collections.Generic.List<GameObject> layerObjectsList
            = new System.Collections.Generic.List<GameObject>();

        for (var i = 0; i < allGameObjectsArray.Length; i++)
        {
            if(allGameObjectsArray[i].layer == thisLayer)
            {
                layerObjectsList.Add(allGameObjectsArray[i]);
            }
        }

        if(layerObjectsList.Count == 0)
        {
            Debug.Log("No objects with layer found");
            return null;
        }

        return layerObjectsList.ToArray();
    }
}