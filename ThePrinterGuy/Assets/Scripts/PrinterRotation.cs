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
        _startAngle = transform.rotation.eulerAngles;
    }

    void Update()
    {

    }
    #endregion

    #region Class Methods
    void SmoothRotation(GameObject go, Vector2 _screenPosition)
    {
        if(go != null && gameObject.Equals(go))
        {
            if(go != null)
            {
                if(!_isBack)
                {
                    iTween.RotateTo(go, iTween.Hash("rotation", _maxAngle,
                                                                "time", _duration));
                    _isBack = true;
                }
                else
                {
                    iTween.RotateTo(go, iTween.Hash("rotation", _startAngle,
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
    #endregion
}
