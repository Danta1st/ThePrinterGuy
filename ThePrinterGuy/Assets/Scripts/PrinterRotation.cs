using UnityEngine;
using System.Collections;

public class PrinterRotation : MonoBehaviour
{
    #region Editor Publics
    [SerializeField]
    private LayerMask _layerMask;
    [SerializeField]
    private GameObject _targetObject;
    [SerializeField]
    private iTween.EaseType _easeType = iTween.EaseType.easeOutBack;
    [SerializeField]
    private Vector3 _maxAngle = Vector3.zero;
    [SerializeField]
    private float _duration = 0.0f;
    #endregion

    #region Privates
    private Camera _camera;
    private RaycastHit _hit;
    private Vector3 _startAngle;
    private bool _isBack = false;
    #endregion

    void OnEnable(){
        GestureManager.OnTap += SmoothRotation;
    }

    void OnDisable(){
        GestureManager.OnTap -= SmoothRotation;
    }

    #region Monobehaviour Functions
    void Start()
    {
        _camera = GameObject.Find("Main Camera").camera;
        _startAngle = transform.localEulerAngles;
    }

    void Update()
    {

    }
    #endregion

    #region Class Methods
    void SmoothRotation(GameObject go, Vector2 _screenPosition)
    {
        Ray _ray = _camera.ScreenPointToRay(_screenPosition);

        if(Physics.Raycast(_ray, out _hit, 100, _layerMask.value))
        {
            if(_hit.collider.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                if(_targetObject != null)
                {
                    if(!_isBack)
                    {
                        iTween.RotateTo(_targetObject, iTween.Hash("rotation", _maxAngle,
                                                                    "time", _duration));
                        _isBack = true;
                    }
                    else
                    {
                        iTween.RotateTo(_targetObject, iTween.Hash("rotation", _startAngle, "easetype", _easeType,
                                                                    "time", _duration));
                        _isBack = false;
                    }
                }
                else
                {
                    Debug.Log("Missing object");
                }
            }
        }
    }
    #endregion
}
